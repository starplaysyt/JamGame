using JamGame.Core;
using JamGame.Graphics;
using JamGame.Graphics.Textures;
using JamGame.UI.Fonts;
using SDL2;
using TDK.Logger;

namespace JamGame.UI;

public class UIButton : IUIControl
{
    private bool _needUpdate = false;
    private string _text;
    private Color _textColor;

    private bool _autoSize;
    
    private AlignmentType _alignment;
    private Point _alignmentMargin;
    
    private Rectangle _shadowRectangle;
    private Rectangle _strokeRectangle;
    
    /// <summary>
    /// Location with connection to ParentControl
    /// </summary>
    private Rectangle _bounds;
    private Rectangle _textLocation;

    private Rectangle _absoluteBounds;
    

    
    public IUIParent? Parent { get; set; }
    public bool IsVisible { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsMouseInside { get; set; } = false;
    public bool IsClicked { get; set; } = false;

    public event EventHandler MouseClick;
    
    public Texture AssignedTexture { get; set; } = new Texture();
    
    public Font AssignedFont { get; set; }
    
    public Color ShadowColor { get; set; }
    public Color TextColor 
    {
        get => _textColor;
        set
        {
            _textColor = value;
            _needUpdate = true;
        }
    }
    public Color BorderColor { get; set; }
    public Color BackgroundColor { get; set; }
    public Color HoverColor { get; set; }
    
    
    public string Text 
    {
        get => _text;
        set
        {
            _needUpdate = true;
            _text = value;
        }
    }
    public int TextSize { get; set; } = 16;

    public Rectangle Bounds
    {
        get => _bounds;
        set
        {
            Rectangle parentParams = new Rectangle();
            if (Parent != null) parentParams = Parent.GetParentParameters();
            
            _bounds = value;
            
            _absoluteBounds = new Rectangle(_bounds.X + parentParams.X, _bounds.Y + parentParams.Y, _bounds.Width, _bounds.Height);
            
            _shadowRectangle = new Rectangle(_absoluteBounds.X, _absoluteBounds.Y, _absoluteBounds.Width + 5, _absoluteBounds.Height +
                5);
            _strokeRectangle = new Rectangle(_absoluteBounds.X - 3, _absoluteBounds.Y - 3, _bounds.Width + 6, _bounds.Height + 6);
            
            
            // ConsoleLogger.LogDebug("UIButton", $"Bounds on button {Text} resetted: current absolute bounds: {_absoluteBounds}");
            // ConsoleLogger.LogDebug("UIButton", $"Bounds on button {Text} resetted: current bounds: {_bounds}");
        }
    }
    
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

    public bool AutoSize
    {
        get => _autoSize;
        set
        {
            _autoSize = value;
            _needUpdate = true;
        }
    }

    public UIButton()
    {
        IsVisible = true;
        IsEnabled = false;
        IsMouseInside = false;
        
        Alignment = AlignmentType.None;
        AlignmentMargin = Point.Zero;
        
        Parent = null;

        AutoSize = true;
        TextColor = new Color(0, 0, 0);
        AssignedFont = FontManager.Fonts["GohuFont"];
        BackgroundColor = new Color(238, 195, 154);

        ShadowColor = new Color(102, 57, 49);
        BorderColor = new Color(0, 0, 0);
        HoverColor = new Color(168, 137, 107);
        BackgroundColor = new Color(238, 195, 154);
    }
    
    public bool CheckMouseInside(Point mousePos)
    {
        if (!IsVisible)
            return false;
        return mousePos.X > _absoluteBounds.X &&
               mousePos.Y < _absoluteBounds.Y + _absoluteBounds.Height &&
               mousePos.X < _absoluteBounds.X + _absoluteBounds.Width &&
               mousePos.Y > _absoluteBounds.Y;
    }

    public bool ProcessEvents(ref SDL.SDL_Event e)
    {
        if (!IsVisible)
            return false;
        switch (e.type)
        {
            case SDL.SDL_EventType.SDL_MOUSEMOTION:
                if (WindowContext.IsMousePressed && IsClicked == false)
                {
                    IsClicked = true;
                    return false;
                }

                return true;
            case SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN:
                IsClicked = true;
                return false;
            case SDL.SDL_EventType.SDL_MOUSEBUTTONUP:
                if (IsClicked)
                {
                    MouseClick?.Invoke(this, EventArgs.Empty);
                    IsClicked = false;
                }
                return false;
        }
        
        return true;
    }

    public void UpdateTextureIfNeeded()
    {
        if (_needUpdate || AssignedTexture.IsAssigned == false)
        {
            WindowContext.Renderer.RendererColor = TextColor;
            AssignedFont.RenderFont(AssignedTexture, TextSize, Text);

            PerformLocationRecalculation();
            
            _textLocation.Width = AssignedTexture.SourceRectangle.Width;
            _textLocation.Height = AssignedTexture.SourceRectangle.Height;
            
            PerformAlignmentRecalculation();
            
            _needUpdate = false;
        }
    }

    public void Draw()
    {
        if (!IsVisible)
            return;
        GraphicsRenderer renderer = WindowContext.Renderer;
        
        UpdateTextureIfNeeded();
        
        renderer.RendererColor = ShadowColor;
        renderer.FillRoundedRectangle(ref _shadowRectangle, 5);
        renderer.RendererColor = BorderColor;
        renderer.FillRoundedRectangle(ref _strokeRectangle, 5);

        if (IsClicked)
            renderer.RendererColor = HoverColor;
        else
            renderer.RendererColor = BackgroundColor;
        renderer.FillRoundedRectangle(ref _absoluteBounds, 5);
        
        renderer.RenderTexture(AssignedTexture, _textLocation);
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
                    AlignmentMargin.X, 
                    AlignmentMargin.Y, 
                    Bounds.Width, Bounds.Height);
                break;
            case AlignmentType.TopRight:
                Bounds = new Rectangle(
                    parentParams.Width - Bounds.Width - AlignmentMargin.X, 
                    AlignmentMargin.Y, 
                    Bounds.Width, Bounds.Height);
                break;
            case AlignmentType.BottomLeft:
                Bounds = new Rectangle(
                    AlignmentMargin.X, 
                    parentParams.Height - Bounds.Height - AlignmentMargin.Y, 
                    Bounds.Width, Bounds.Height);
                break;
            case AlignmentType.BottomRight:
                Bounds = new Rectangle(
                    parentParams.Width - Bounds.Width - AlignmentMargin.X,
                    parentParams.Height - Bounds.Height - AlignmentMargin.Y, 
                    Bounds.Width, Bounds.Height);
                break;
        }
        
        PerformLocationRecalculation();
    }
    
    public void PerformLocationRecalculation()
    {
        if (AutoSize)
        {
            Bounds = new Rectangle(Bounds.X, Bounds.Y, AssignedTexture.SourceRectangle.Width, AssignedTexture.SourceRectangle.Height);
            // ConsoleLogger.LogDebug("UIButton", $"{Text} : textlocautosizePlus {_textLocation}");
            _textLocation.X = _absoluteBounds.X + 3;
            _textLocation.Y = _absoluteBounds.Y + 3;
            // ConsoleLogger.LogDebug("UIButton", $"{Text} : textlocautosize {_textLocation}");
        }
        else
        {
            // ConsoleLogger.LogDebug("UIButton", $"{Text} : locrecalc {Bounds}");
            _textLocation.X = _absoluteBounds.X + Bounds.Width / 2 - AssignedTexture.SourceRectangle.Width / 2;;
            _textLocation.Y = _absoluteBounds.Y + Bounds.Height / 2 - AssignedTexture.SourceRectangle.Height / 2;
            // ConsoleLogger.LogDebug("UIButton", $"{Text} : locrecalc fin {Bounds}");
        }
    }
}