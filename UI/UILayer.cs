using JamGame.Core;
using JamGame.Graphics;
using JamGame.UI.Fonts;
using SDL2;

namespace JamGame.UI;

/// <summary>
/// Home of all user interface in game
/// </summary>
public class UILayer : ILayer
{
    public List<IControl> Controls = new List<IControl>();
    public string Name { get; set; } = "UILayer";
    public bool IsVisible { get; set; } = true;
    
    public bool IsActiveLayer { get; set; }

    public IControl? SelectedControl { get; set; } = null;

    public IControl? MouseInsideControl { get; set; } = null;

    private UIButton _testButton;
    
    public UILayer()
    {
        _testButton = new UIButton()
        {
            AssignedFont = FontManager.Fonts["GohuFont"],
            Bounds = new Rectangle(100, 100, 200, 100),
            Text = "GohuFontTest",
            TextSize = 20
        };
        
        Controls.Add(_testButton);
    }

    public void Draw()
    {
        foreach (var control in Controls)
        {
            if (control.IsVisible)
                control.Draw();
        }
    }

    public bool FindMouseInside(Point point)
    {
        foreach (var control in Controls)
        {
            if (control.CheckMouseInside(point))
            {
                if (control == MouseInsideControl)
                    return true;
                
                if (MouseInsideControl != null)
                    MouseInsideControl.IsMouseInside = false;
                
                MouseInsideControl = control;
                MouseInsideControl.IsMouseInside = true;
                return true;
            }
        }
        
        return false;
    }

    public void UpdateByTick()
    {
        
    }

    public bool UpdateEvents(ref SDL.SDL_Event e)
    {
        //Console.WriteLine("Got update on UI");
        if (SelectedControl != null)
        {
            return SelectedControl.ProcessEvents(ref e);
        }
        
        if (MouseInsideControl != null)
        {
            if (FindMouseInside(WindowContext.MouseLocation))
            {
                return MouseInsideControl.ProcessEvents(ref e);
                
            }
        }
        else
        {
            if (e.type == SDL.SDL_EventType.SDL_MOUSEMOTION)
            {
                if (FindMouseInside(new Point(e.motion.x, e.motion.y)))
                {
                    return MouseInsideControl.ProcessEvents(ref e);
                }
            }
        }
        
        //Console.WriteLine("No update on UI");
        return true;
    }
}
