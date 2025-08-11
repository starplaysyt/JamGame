using JamGame.Core;
using JamGame.Graphics;
using SDL2;

namespace JamGame.UI;

public class UIPanel : IUIControl, IUIParent
{
    private Rectangle _shadowRectangle;
    private Rectangle _strokeRectangle;
    private Rectangle _absoluteBounds;
    private Rectangle _bounds;
    
    private AlignmentType _alignment;
    private Point _alignmentMargin;
    
    public AlignmentType Alignment
    {
        get => _alignment;
        set
        {
            _alignment = value;
            PerformAlignmentRecalculation();
        }
    } 

    public Point AlignmentMargin { get; set; }
    
    public IUIParent? Parent { get; set; }
    public bool IsVisible { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsMouseInside { get; set; }

    public List<IUIControl> Children = new List<IUIControl>();

    public IUIControl? MouseInsideControl = null;
    
    public Color ShadowColor { get; set; }
    public Color BorderColor { get; set; }
    public Color BackgroundColor { get; set; }
    
    public Rectangle Bounds
    {
        get => _bounds;
        set
        {
            Rectangle parentParams = new Rectangle();
            
            if (Parent != null)
                parentParams = Parent.GetParentParameters();
            
            _shadowRectangle = new Rectangle(value.X + 2 + parentParams.X, value.Y + 2 + parentParams.Y, 
                value.Width - 2, value.Height - 2);
            _strokeRectangle = new Rectangle(value.X - 3  + parentParams.X, value.Y - 3 + parentParams.Y,
                value.Width, value.Height);
            _bounds = new Rectangle(value.X, value.Y, 
                value.Width, value.Height);

            _absoluteBounds = new Rectangle(_bounds)
            {
                X = _bounds.X + parentParams.X,
                Y = _bounds.Y + parentParams.Y,
                Width = _bounds.Width - 6,
                Height = _bounds.Height - 6,
            };
        }
    }

    public UIPanel()
    {
        IsVisible = true;
        IsEnabled = false;
        IsMouseInside = false;
        
        Alignment = AlignmentType.None;
        AlignmentMargin = Point.Zero;
        
        Parent = null;
        
        Bounds = new Rectangle(0, 0, 200, 100);
        
        ShadowColor = new Color(102, 57, 49);
        BorderColor = new Color(0, 0, 0);
        BackgroundColor = new Color(238, 195, 154);
    }
    
    public bool CheckMouseInside(Point mousePos)
    {
        return mousePos.X > _absoluteBounds.X &&
               mousePos.Y < _absoluteBounds.Y + _absoluteBounds.Height &&
               mousePos.X < _absoluteBounds.X + _absoluteBounds.Width &&
               mousePos.Y > _absoluteBounds.Y;
    }
    
    public bool FindMouseInside(Point point)
    {
        if (!IsVisible)
            return false;
        
        foreach (var control in Children)
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

    public bool ProcessEvents(ref SDL.SDL_Event e)
    {
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

    public void Draw()
    {
        if (!IsVisible)
            return;
        
        GraphicsRenderer renderer = WindowContext.Renderer;
        
        renderer.RendererColor = ShadowColor;
        renderer.FillRoundedRectangle(ref _shadowRectangle, 5);
        renderer.RendererColor = BorderColor;
        renderer.FillRoundedRectangle(ref _strokeRectangle, 5);
        renderer.RendererColor = BackgroundColor;
        renderer.FillRoundedRectangle(ref _absoluteBounds, 5);

        foreach (var child in Children)
        {
            child.Draw();
        }
    }

    public void PerformAlignmentRecalculation()
    {
        if (Alignment == AlignmentType.None || Parent == null)
            return;

        var parentParams = Parent.GetParentParameters();
        
        switch (Alignment)
        {
            case AlignmentType.TopLeft:
                Bounds = new Rectangle(
                    parentParams.Location.X + AlignmentMargin.X, 
                    parentParams.Location.Y + AlignmentMargin.Y, 
                    Bounds.Width, Bounds.Height);
                break;
            case AlignmentType.TopRight:
                Bounds = new Rectangle(
                    parentParams.Location.X + parentParams.Width - Bounds.Width - AlignmentMargin.X, 
                    parentParams.Location.Y + AlignmentMargin.Y, 
                    Bounds.Width, Bounds.Height);
                break;
            case AlignmentType.BottomLeft:
                Bounds = new Rectangle(
                    parentParams.Location.X + AlignmentMargin.X, 
                    parentParams.Location.Y + parentParams.Height - Bounds.Height - AlignmentMargin.Y, 
                    Bounds.Width, Bounds.Height);
                break;
            case AlignmentType.BottomRight:
                Bounds = new Rectangle(
                    parentParams.Location.X + parentParams.Width - Bounds.Width - AlignmentMargin.X, 
                    parentParams.Location.Y + parentParams.Height - Bounds.Height - AlignmentMargin.Y, 
                    Bounds.Width, Bounds.Height);
                break;
        }
        
        PerformLocationRecalculation();
    }

    public void RegisterControl(IUIControl control)
    {
        Children.Add(control);
        control.Parent = this;
        
        Bounds = _bounds;
        PerformAlignmentRecalculation();
    }

    public void PerformLocationRecalculation()
    {
        foreach (var child in Children)
        {
            child.PerformAlignmentRecalculation();
        }
    }

    public Rectangle GetParentParameters()
    {
        return _absoluteBounds;
    }
}