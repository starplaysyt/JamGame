using JamGame.Core;
using JamGame.Graphics;
using JamGame.UI.Fonts;
using SDL2;

namespace JamGame.UI;

/// <summary>
/// Home of all user interface in game
/// </summary>
public class UILayer : ILayer, IUIParent
{
    public List<IUIControl> Controls = new List<IUIControl>();
    public string Name { get; set; } = "UILayer";
    public bool IsVisible { get; set; } = true;
    
    public bool IsActiveLayer { get; set; }

    public IUIControl? SelectedControl { get; set; } = null;

    public IUIControl? MouseInsideControl { get; set; } = null;

    private UIButton _testButton;

    private UILabel _label;

    private UIPanel _panel;
    
    public UILayer()
    {
        _testButton = new UIButton()
        {
            Parent = this,
            AssignedFont = FontManager.Fonts["GohuFont"],
            Bounds = new Rectangle(100, 100, 200, 100),
            Text = "GohuFontTest",
            AutoSize = true,
            TextSize = 20,
            Alignment = AlignmentType.BottomRight,
            AlignmentMargin = new Point(10,60)
        };

        _label = new UILabel()
        {
            Parent = this,
            AssignedFont = FontManager.Fonts["GohuFont"],
            Bounds = new Rectangle(400, 400, 50, 60),
            BackgroundColor = Color.Transparent,
            TextColor = Color.White,
            Text = "TestUILabel",
            TextSize = 15,
            Alignment = AlignmentType.BottomRight,
            AlignmentMargin = new Point(10,10)
        };

        _panel = new UIPanel()
        {
            Parent = this,
            Bounds = new Rectangle(60, 60, 200, 100),
            Alignment = AlignmentType.TopLeft,
            AlignmentMargin = new(20, 20)
        };

        UIButton b1 = new UIButton()
        {
            Text = "testText",
            TextColor = Color.White,
            Bounds = new Rectangle(50, 50, 80, 20),
            AutoSize = false,
            Alignment = AlignmentType.BottomRight,
            AlignmentMargin = new Point(5, 5)
        };
        
        _panel.RegisterControl(b1);
        
        
        Controls.Add(_label);
        Controls.Add(_testButton);
        Controls.Add(_panel);
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
        if (e.type == SDL.SDL_EventType.SDL_WINDOWEVENT)
        {
            if (e.window.windowEvent == SDL.SDL_WindowEventID.SDL_WINDOWEVENT_RESIZED)
            {
                foreach (var control in Controls)
                    control.PerformAlignmentRecalculation();
                return true;
            }
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

    public Rectangle GetParentParameters() 
    {
        return new Rectangle(Point.Zero, WindowContext.WindowSize);
    }
}
