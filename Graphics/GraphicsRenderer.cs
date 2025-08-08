using JamGame.Core;
using JamGame.Graphics.Textures;
using SDL2;
using TDK.Logger;
using static SDL2.SDL;

namespace JamGame.Graphics;

public class GraphicsRenderer
{
    private Color _rendererColor;
    private IntPtr _sdlRenderer;
    
    public BlendMode BlendMode 
    {
        get
        {
            SDL_BlendMode blendMode;
            SDL_GetRenderDrawBlendMode(_sdlRenderer, out blendMode);
            return blendMode switch
            {
                SDL_BlendMode.SDL_BLENDMODE_NONE => BlendMode.None,
                SDL_BlendMode.SDL_BLENDMODE_BLEND => BlendMode.Blend,
                SDL_BlendMode.SDL_BLENDMODE_ADD => BlendMode.Add,
                SDL_BlendMode.SDL_BLENDMODE_MOD => BlendMode.Mod,
                SDL_BlendMode.SDL_BLENDMODE_INVALID => BlendMode.None,
                SDL_BlendMode.SDL_BLENDMODE_MUL => BlendMode.Add,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        set
        {
            switch (value)
            {
                case BlendMode.None:
                    SDL_SetRenderDrawBlendMode(_sdlRenderer, SDL_BlendMode.SDL_BLENDMODE_NONE);
                    break;
                case BlendMode.Blend:
                    SDL_SetRenderDrawBlendMode(_sdlRenderer, SDL_BlendMode.SDL_BLENDMODE_BLEND);
                    break;
                case BlendMode.Mod:
                    SDL_SetRenderDrawBlendMode(_sdlRenderer, SDL_BlendMode.SDL_BLENDMODE_MOD);
                    break;
                case BlendMode.Add:
                    SDL_SetRenderDrawBlendMode(_sdlRenderer, SDL_BlendMode.SDL_BLENDMODE_ADD);
                    break;
                default:
                    throw new Exception($"Unsupported blend mode: {value}");
            }
        }
    }

    internal Color RendererColor 
    {
        get => _rendererColor;
        set
        {
            _rendererColor = value;
            SDL_SetRenderDrawColor(_sdlRenderer, _rendererColor.R, _rendererColor.G, _rendererColor.B, _rendererColor.A);
        }
    }

    public IntPtr SDLRenderer { get => _sdlRenderer; }

    private Texture? _rendererTarget;
    
    public Texture? RendererTarget 
    {
        get
        {
            return _rendererTarget;
        }
        set
        {
            if (value == null)
            {
                SDL_SetRenderTarget(SDLRenderer, IntPtr.Zero);
                ClearRenderer();
            }
            else
                SDL_SetRenderTarget(SDLRenderer, value.SDLTexture);
            
            _rendererTarget = value;
        }
    }

    internal GraphicsRenderer(IntPtr window)
    {
        _sdlRenderer = SDL_CreateRenderer(window, -1, SDL_RendererFlags.SDL_RENDERER_ACCELERATED);
        if (_sdlRenderer == IntPtr.Zero)
            ConsoleLogger.LogFatal("WindowHandler", $"Failed to create renderer for window with id: {SDL_GetWindowID(window)}. See more in: {SDL_GetError()}");
    } 

    internal void RenderComplete() 
    {
        SDL_RenderPresent(_sdlRenderer);
        SDL_RenderClear(_sdlRenderer);
    }
    
    public void ClearRenderer() => SDL_RenderClear(_sdlRenderer);

    internal void RenderTexture(Texture texture, Rectangle destinationRectangle)
    {
        if (texture == RendererTarget)
        {
            ConsoleLogger.LogError("StaticRenderer", "Tried to render texture on itself, what is prohibited.");
            return;
        }

        var srcRect = texture.SourceRectangle.ToSDL(); 
        var dstRect = destinationRectangle.ToSDL();
        
        if (SDL_RenderCopy(SDLRenderer, texture.SDLTexture, ref srcRect, ref dstRect) != 0)
            ConsoleLogger.LogFatal("StaticRenderer", $"Failed to render texture, Error: {SDL_GetError()}");
    }
    
    public void DrawPixel(int x, int y) => SDL_RenderDrawPoint(_sdlRenderer, x, y);
    public void DrawPixel(ref Point position) => SDL_RenderDrawPoint(_sdlRenderer, position.X, position.Y);
    
    public void DrawLine(int x1, int y1, int x2, int y2) => SDL_RenderDrawLine(_sdlRenderer, x1, y1, x2, y2);
    public void DrawLine(ref Point p1, ref Point p2) => SDL_RenderDrawLine(_sdlRenderer, p1.X, p1.Y, p2.X, p2.Y);
    
    public void DrawRectangle(int x, int y, int w, int h)
    {
        var rect = new SDL_Rect() {x = x, y = y, w = w, h = h};
        SDL_RenderDrawRect(_sdlRenderer, ref rect);
    }
    public void DrawRectangle(ref Rectangle rectangle) 
    {
        var sdlRect = rectangle.ToSDL();
        SDL_RenderDrawRect(_sdlRenderer, ref sdlRect);
    }

    public void DrawCircle(int x, int y, int radius) 
    {
        int diameter = radius * 2, dx = radius - 1, dy = 0, tx = 1, ty = 1, error = tx - diameter;
        int retvals = 0;

        while (dx >= dy)
        {
            retvals += SDL_RenderDrawPoint(_sdlRenderer, x + dx, y + dy);
            retvals += SDL_RenderDrawPoint(_sdlRenderer, x + dx, y - dy);
            retvals += SDL_RenderDrawPoint(_sdlRenderer, x - dx, y + dy);
            retvals += SDL_RenderDrawPoint(_sdlRenderer, x - dx, y - dy);
            retvals += SDL_RenderDrawPoint(_sdlRenderer, x + dy, y - dx);
            retvals += SDL_RenderDrawPoint(_sdlRenderer, x + dy, y + dx);
            retvals += SDL_RenderDrawPoint(_sdlRenderer, x - dx, y + dy);
            retvals += SDL_RenderDrawPoint(_sdlRenderer, x - dx, y - dy);
            
            if (error <= 0)
            {
                ++dy;
                error += ty;
                ty += 2;
            }
            else 
            {
                --dx;
                tx += 2;
                error += (tx - diameter);
            }
        }
    }
    public void DrawCircle(ref Point location, int radius) => DrawCircle(location.X, location.Y, radius);

    public void FillRectangle(int x, int y, int w, int h)
    {
        var rect = new SDL_Rect() {x = x, y = y, w = w, h = h};
        SDL_RenderFillRect(_sdlRenderer, ref rect);
    }
    public void FillRectangle(ref Rectangle rectangle) 
    {
        var sdlRect = rectangle.ToSDL();
        SDL_RenderFillRect(_sdlRenderer, ref sdlRect);
    }
    
    public void FillCircle(int x, int y, int radius) 
    {
        int retvals = 0;
        for (int i = -radius; i < radius; i++)
        {
            var countedY = (int)Math.Sqrt(radius * radius - i * i);
            retvals += SDL_RenderDrawLine(_sdlRenderer, x + i, y - countedY, x + i, y);
            retvals += SDL_RenderDrawLine(_sdlRenderer, x + i, y + countedY, x + i, y + 1);
        }
    }
    public void FillCircle(ref Point location, int radius) => FillCircle(location.X, location.Y, radius);
    
    public void DrawArc(int x, int y, int radius, ArcSegment segment) 
    {
        int diameter = radius * 2, dx = radius - 1, dy = 0, tx = 1, ty = 1, error = tx - diameter;
        switch (segment)
        {
            case ArcSegment.UpLeft:
                while (dx >= dy)
                {
                    SDL_RenderDrawPoint(_sdlRenderer, x - dy, y - dx);
                    SDL_RenderDrawPoint(_sdlRenderer, x - dx, y - dy);
            
                    if (error <= 0)
                    {
                        ++dy;
                        error += ty;
                        ty += 2;
                    }
                    else 
                    {
                        --dx;
                        tx += 2;
                        error += (tx - diameter);
                    }
                }
                return;
            case ArcSegment.UpRight:
                while (dx >= dy)
                {
                    SDL_RenderDrawPoint(_sdlRenderer,x + dy, y - dx);
                    SDL_RenderDrawPoint(_sdlRenderer,x + dx, y - dy);
            
                    if (error <= 0)
                    {
                        ++dy;
                        error += ty;
                        ty += 2;
                    }
                    else 
                    {
                        --dx;
                        tx += 2;
                        error += (tx - diameter);
                    }
                }
                return;
            case ArcSegment.DownLeft:
                while (dx >= dy)
                {
                    SDL_RenderDrawPoint(_sdlRenderer,x - dx, y + dy);
                    SDL_RenderDrawPoint(_sdlRenderer,x - dy, y + dx);
            
                    if (error <= 0)
                    {
                        ++dy;
                        error += ty;
                        ty += 2;
                    }
                    else 
                    {
                        --dx;
                        tx += 2;
                        error += (tx - diameter);
                    }
                }
                return;
            case ArcSegment.DownRight:
                while (dx >= dy)
                {
                    SDL_RenderDrawPoint(_sdlRenderer,x + dy, y + dx);
                    SDL_RenderDrawPoint(_sdlRenderer,x + dx, y + dy);
                    
                    if (error <= 0)
                    {
                        ++dy;
                        error += ty;
                        ty += 2;
                    }
                    else 
                    {
                        --dx;
                        tx += 2;
                        error += (tx - diameter);
                    }
                }
                return;
        }
    }
    public void DrawArc(ref Point location, int radius, ArcSegment segment) => DrawArc(location.X, location.Y, radius, segment);

    public void FillArc(int x, int y, int radius, ArcSegment segment)
    {
        int countedY;
        switch (segment)
        {
            case ArcSegment.UpLeft:
                for (var i = -radius + 1; i < 0; i++)
                {
                    countedY = (int)Math.Sqrt(radius * radius - i * i);
                    SDL_RenderDrawLine(_sdlRenderer, x + i, y - countedY - 1, x + i - 1, y);
                }
                return;
            case ArcSegment.UpRight:
                for (var i = 1; i < radius; i++)
                {
                    countedY = (int)Math.Sqrt(radius * radius - i * i);
                    SDL_RenderDrawLine(_sdlRenderer,x + i + 1, y - countedY - 1, x + i + 1, y);
                }
                return;
            case ArcSegment.DownLeft:
                for (var i = -radius + 1; i < 0; i++)
                {
                    countedY = (int)Math.Sqrt(radius * radius - i * i);
                    SDL_RenderDrawLine(_sdlRenderer,x + i - 1, y + countedY + 1, x + i - 1, y);
                }
                return;
            case ArcSegment.DownRight:
                for (var i = 1; i < radius; i++)
                {
                    countedY = (int)Math.Sqrt(radius * radius - i * i);
                    SDL_RenderDrawLine(_sdlRenderer,x + i + 1, y + countedY + 1, x + i + 1, y);
                }
                return;
            default:
                ConsoleLogger.LogError("StaticRenderer.cs", $"DrawArc called with unsupported ArcSegment - {segment}");
                break;
        }
    }
    public void FillArc(ref Point location, int radius, ArcSegment segment) => FillArc(location.X, location.Y, radius, segment);
    
    public void DrawRoundedRectangle(int x, int y, int w, int h, int cornerRadius)
    {
        if (cornerRadius == 0)
        {
            DrawRectangle(x, y, w, h);
            return;
        }
        
        DrawArc(x - 1 + cornerRadius, y + cornerRadius - 1, cornerRadius, ArcSegment.UpLeft);
        DrawArc(x + 1 + w - cornerRadius, y + cornerRadius - 1, cornerRadius, ArcSegment.UpRight);
        DrawArc(x + cornerRadius - 1, y + h - cornerRadius + 1, cornerRadius, ArcSegment.DownLeft);
        DrawArc(x + w - cornerRadius + 1, y + h - cornerRadius + 1, cornerRadius, ArcSegment.DownRight);
        
        DrawLine(x + cornerRadius, y, x + w - cornerRadius, y);
        
        DrawLine(x, y + cornerRadius, x, y + h - cornerRadius);
        
        DrawLine(x + cornerRadius, y + h, x + w - cornerRadius, y + h);
        
        DrawLine(x + w, y + cornerRadius, x + w, y + h - cornerRadius);
    }
    public void DrawRoundedRectangle(ref Rectangle rect, int cornerRadius) => 
        DrawRoundedRectangle(rect.X, rect.Y, rect.Width, rect.Height, cornerRadius);

    public void FillRoundedRectangle(int x, int y, int w, int h, int cornerRadius)
    {
        if (cornerRadius == 0)
        {
            FillRectangle(x, y, w, h);
            return;
        }
        
        FillArc(x + cornerRadius, y + cornerRadius, cornerRadius, ArcSegment.UpLeft);
        FillArc(x + w - cornerRadius, y + cornerRadius, cornerRadius, ArcSegment.UpRight);
        FillArc(x + cornerRadius, y + h - cornerRadius, cornerRadius, ArcSegment.DownLeft);
        FillArc(x + w - cornerRadius, y + h - cornerRadius, cornerRadius, ArcSegment.DownRight);
        
        FillRectangle(x + cornerRadius - 1, y, 
            w - 2 * cornerRadius + 3, h + 1); //central rectangle
        
        FillRectangle(x, y + cornerRadius + 1, 
            cornerRadius - 1, h - 2 * cornerRadius - 1); //left rectangle
        
        FillRectangle(x + w - cornerRadius + 2, y + cornerRadius + 1, 
            cornerRadius - 1, h - 2 * cornerRadius - 1); //right rectangle
    }
    public void FillRoundedRectangle(ref Rectangle rect, int cornerRadius) =>
        FillRoundedRectangle(rect.X, rect.Y, rect.Width, rect.Height, cornerRadius);
    
    public enum ArcSegment
    {
        UpLeft, UpRight, DownLeft, DownRight
    }
}