using JamGame.Core;
using SDL2;

namespace JamGame.UI;

public interface IControl
{
    public bool IsVisible { get; set; }
    
    public bool IsEnabled { get; set; }
    
    public bool IsMouseInside { get; set; }

    public void Draw()
    {
        
    }

    public bool CheckMouseInside(Point mousePos)
    {
        return false;
    }

    public bool ProcessEvents(ref SDL.SDL_Event e)
    {
        return true;
    }
}