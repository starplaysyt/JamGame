using SDL2;

namespace JamGame.Graphics;

/// <summary>
/// public rendering interface
/// </summary>
public class WindowContext
{
    private IntPtr _windowTarget;
    
    private GraphicsRenderer _graphics;

    public WindowContext()
    {
        _windowTarget = SDL.SDL_CreateWindow("testwindow", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED,
            1080, 720, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);
        
        
        _graphics = new GraphicsRenderer(_windowTarget);
    }

    public int GetSDLEvent(out SDL.SDL_Event evt)
    {
        return SDL.SDL_PollEvent(out evt);
    }
}