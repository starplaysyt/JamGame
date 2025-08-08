using SDL2;
using static SDL2.SDL;
using TDK.Logger;

namespace JamGame.Graphics;

/// <summary>
/// public rendering interface
/// </summary>
public static class WindowContext
{
    private static IntPtr _windowTarget;
    
    public static GraphicsRenderer Renderer;
    static WindowContext()
    {
        if (SDL_Init(SDL_INIT_EVERYTHING) != 0)
        {
            ConsoleLogger.LogError("Graphics Renderer", "Cannot initialize SDL via Init.");
            ConsoleLogger.LogError("Graphics Renderer", "Contact with developers. ");
            ConsoleLogger.LogError("Graphics Renderer", "Reason(error): " + SDL_GetError());
            ConsoleLogger.LogFatal("Graphics Renderer", "Game crashed with fatal error.");
        }

        SDL_ttf.TTF_Init();
        SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_PNG);
        
        _windowTarget = SDL.SDL_CreateWindow("testwindow", SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED,
            1080, 720, SDL.SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE);

        Renderer = new GraphicsRenderer(_windowTarget);
    }

    public static int GetSDLEvent(out SDL.SDL_Event evt)
    {
        return SDL.SDL_PollEvent(out evt);
    }
}