using JamGame.Core;
using SDL2;

namespace JamGame.Graphics;

public class PlayerCamera : ILayer
{
    /// <summary>
    /// Map Location with zoom shift
    /// </summary>
    public FPoint MapLocation { get; set; } = new();
    
    /// <summary>
    /// Mouse location on screen, with camera location shift
    /// </summary>
    public Point MouseAbsoluteLocation { get; set; } = new();

    /// <summary>
    /// Mouse location on map, in pixels, without zoom, right on texture.
    /// </summary>
    public Point MouseMapLocation { get; set; } = new();

    public int MovementSpeed { get; set; } = 3;

    public float Zoom { get; set; } = 1;

    public bool isMovingUpwards = false;
    public bool isMovingDownwards = false;
    public bool isMovingLeft = false;
    public bool isMovingRight = false;

    public PlayerCamera()
    {
    }

    public string Name { get; set; } = "PlayerCamera";
    public bool IsVisible { get; set; } = true;

    public void Draw()
    {
        GraphicsRenderer renderer = WindowContext.Renderer;
    }

    public void UpdateByTick()
    {
        if (isMovingUpwards)
        {
            MapLocation = new FPoint(MapLocation.X, MapLocation.Y + MovementSpeed);
        }

        if (isMovingDownwards)
        {
            MapLocation = new FPoint(MapLocation.X, MapLocation.Y - MovementSpeed);
        }

        if (isMovingLeft)
        {
            MapLocation = new FPoint(MapLocation.X + MovementSpeed, MapLocation.Y);
        }

        if (isMovingRight)
        {
            MapLocation = new FPoint(MapLocation.X - MovementSpeed, MapLocation.Y);
        }
    }

    public bool UpdateEvents(ref SDL.SDL_Event e)
    {
        switch (e.type)
        {
            case SDL.SDL_EventType.SDL_KEYDOWN:
                switch (e.key.keysym.sym)
                {
                    case SDL.SDL_Keycode.SDLK_w:
                        isMovingUpwards = true;
                        break;
                    case SDL.SDL_Keycode.SDLK_a:
                        isMovingLeft = true;
                        //left, x-
                        return false;
                    case SDL.SDL_Keycode.SDLK_s:
                        isMovingDownwards = true;
                        //backwars y-
                        return false;
                    case SDL.SDL_Keycode.SDLK_d:
                        isMovingRight = true;
                        //right, x+
                        return false;
                    case SDL.SDL_Keycode.SDLK_LSHIFT:
                        MovementSpeed = 6;
                        break;
                }

                break;
            case SDL.SDL_EventType.SDL_KEYUP:
                switch (e.key.keysym.sym)
                {
                    case SDL.SDL_Keycode.SDLK_w:
                        isMovingUpwards = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_a:
                        isMovingLeft = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_s:
                        isMovingDownwards = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_d:
                        isMovingRight = false;
                        break;
                    case SDL.SDL_Keycode.SDLK_LSHIFT:
                        MovementSpeed = 3;
                        break;
                }

                break;
            
            case SDL.SDL_EventType.SDL_MOUSEMOTION:
                MouseAbsoluteLocation = new Point(                    
                    WindowContext.MouseLocation.X - (int)MapLocation.X,
                    WindowContext.MouseLocation.Y - (int)MapLocation.Y
                );
                
                MouseMapLocation = new Point(
                    (int)(MouseAbsoluteLocation.X / Zoom),
                    (int)(MouseAbsoluteLocation.Y / Zoom)
                    );
                
                break;
            
            case SDL.SDL_EventType.SDL_MOUSEWHEEL:
                if (e.wheel.y > 0)
                    Zoom = Math.Min(Zoom + 0.05f * Zoom, 5.0f);
                else
                    Zoom = Math.Max(Zoom - 0.05f * Zoom, 0.3f);

                Point mouseMapPrev = MouseMapLocation;
                
                MouseMapLocation = new Point(
                    (int)(MouseAbsoluteLocation.X / Zoom),
                    (int)(MouseAbsoluteLocation.Y / Zoom)
                );
                
                int deltaX = mouseMapPrev.X - MouseMapLocation.X;
                int deltaY = mouseMapPrev.Y - MouseMapLocation.Y;
                
                MapLocation = new FPoint(MapLocation.X - deltaX * Zoom, MapLocation.Y - deltaY * Zoom);
                
                MouseAbsoluteLocation = new Point(                    
                    WindowContext.MouseLocation.X - (int)MapLocation.X,
                    WindowContext.MouseLocation.Y - (int)MapLocation.Y
                );
                
                MouseMapLocation = new Point(
                    (int)(MouseAbsoluteLocation.X / Zoom),
                    (int)(MouseAbsoluteLocation.Y / Zoom)
                );
                
                
                
                break;
        }

        return true;
    }

    public void GetController(LayerController controller)
    {
        controller.LayerZoom = Zoom;
        controller.PixelLocation = MapLocation.ToPoint();
    }

    public void RegisterGraphics(IDrawable drawable)
    {
        throw new NotImplementedException();
    }
}