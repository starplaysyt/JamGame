using JamGame.Core;
using SDL2;

namespace JamGame.UI;

public interface IUIControl
{
    IUIParent? Parent { get; set; }
    
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

    /// <summary>
    /// This shit might use WindowContexts' WindowSize field for that stuff. UI thing only
    /// </summary>
    public void PerformAlignmentRecalculation()
    {
        
    }
}