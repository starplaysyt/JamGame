using JamGame.Core;
using JamGame.Graphics;
using JamGame.Graphics.Textures;
using static SDL2.SDL_ttf;
using static SDL2.SDL;

using TDK.Logger;

namespace JamGame.UI.Fonts;

public class Font
{
    private IntPtr _fontPtr;
    
    public string Name { get; private set; }
    
    public string FontFamily { get; private set; }
    
    public string WayToFontFile { get; private set; }
    
    public Font(string wayToFontFile)
    {
        _fontPtr = TTF_OpenFont(wayToFontFile, 10);

        if (_fontPtr == IntPtr.Zero)
        {
            ConsoleLogger.LogError("Font", $"Failed to open font file \"{wayToFontFile}\". See more: {TTF_GetError()}");
            return;
        }
        
        FontFamily = TTF_FontFaceFamilyName(_fontPtr);
        Name = TTF_FontFaceStyleName(_fontPtr);
        
        WayToFontFile = wayToFontFile;
    }

    /// <summary>
    /// Renders font on texture. Deletes old texture if there is so necessity, and set it with a new one.
    /// </summary>
    /// <param name="texture">Not-null texture, destination of rendering</param>
    /// <param name="renderer">Renderer, what will render text</param>
    /// <param name="text">Text to render</param>
    /// <param name="fontSize">Font size</param>
    /// <param name="wrapSize">Sets text borders. When 0 set no borders, but support \n </param>
    /// <exception cref="NullReferenceException"></exception>
    public void RenderFont(Texture texture, int fontSize,
        string text, int wrapSize = 0)
    {
        if (texture is null) //Just to keep this safe
        {
            ConsoleLogger.LogFatal("Font", $"Tried to render text {text} with font {Name} when texture is null");
            return;
        }
        
        if (text.Length == 0) //When need to render empty text returns empty texture with special height
        {
            texture.DeleteTexture();
            texture.CreateTexture(WindowContext.Renderer, TextureType.Static, new Size(0, fontSize));
            return;
        }

        var renderColor = WindowContext.Renderer.RendererColor.ToSDL();
        
        TTF_SetFontSize(_fontPtr, fontSize);
        
        var sdlSurface = TTF_RenderUNICODE_Blended_Wrapped(_fontPtr, 
            text, renderColor, Convert.ToUInt32(wrapSize));
        if (sdlSurface == IntPtr.Zero)
        {
            ConsoleLogger.LogError("Font", 
                $"Failed to create font surface. Font: {Name}, Text: {text}. More info: {TTF_GetError()}");
            return;
        }
        
        WindowContext.Renderer.BlendMode = BlendMode.Blend; //I totally don't know why this is there, but is's just works
        var newTexture = SDL_CreateTextureFromSurface(WindowContext.Renderer.SDLRenderer, sdlSurface);
        if (newTexture == IntPtr.Zero)
        {
            ConsoleLogger.LogError("Font", 
                $"Failed to create font texture. Font: {Name}, Text: {text}. More info: {TTF_GetError()}");
            return;
        }
        
        int w, h;
        SDL_QueryTexture(newTexture, out _, out _, out w, out h);
        SDL_FreeSurface(sdlSurface);
        
        if (texture.IsAssigned) texture.DeleteTexture();
        texture.AssignTexture(newTexture, TextureType.Static, new Rectangle {Width = w, Height = h});
    }

    public int GetTextWidth(string text)
    {
        if (TTF_SizeUNICODE(_fontPtr, text, out var w, out _) != 0)
        {
            ConsoleLogger.LogError("Font.cs",$"Failed to get text width of font {Name}, more info: {TTF_GetError()}.");
            return 0;
        }
        
        return w;
    }

    public int GetTextHeight(string text)
    {
        if (TTF_SizeUNICODE(_fontPtr, text, out _, out var h) != 0)
        {
            ConsoleLogger.LogError("Font.cs",$"Failed to get text height of font {Name}, more info: {TTF_GetError()}.");
            return 0;
        }
        return h;
    }
    
    ~Font()
    {
        TTF_CloseFont(_fontPtr);
    }
}