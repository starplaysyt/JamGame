using JamGame.Core;
using JamGame.Graphics;
using JamGame.Graphics.Textures;
using JamGame.UI.Fonts;
using SDL2;
using TDK.Logger;

namespace JamGame.UI;

public class UILabel : IUIControl
{
    private bool _needUpdate = false;
    private string _text;
    private Color _textColor;
    private bool _autoSize;
    private AlignmentType _alignment;
    private Point _alignmentMargin;
    
    private Rectangle _textLocation;

    private Rectangle _bounds;
    
    public IUIParent? Parent { get; set; }
    
    public Color TextColor
    {
        get => _textColor;
        set
        {
            _textColor = value;
            _needUpdate = true;
        }
    }

    public Color BackgroundColor { get; set; }
    
    public bool IsVisible { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsMouseInside { get; set; } = false;

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
    
    public Texture AssignedTexture { get; set; } = new Texture();
    
    public Font AssignedFont { get; set; }

    public bool AutoSize
    {
        get => _autoSize;
        set
        {
            _autoSize = value;
            _needUpdate = true;
        }
    }
    
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
            _bounds = value;
            if (!AutoSize)
                PerformAlignmentRecalculation();   
        }
    }

    public UILabel()
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
    }
    public bool CheckMouseInside(Point mousePos)
    {
        return false;
    }

    public bool ProcessEvents(ref SDL.SDL_Event e)
    {
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
        GraphicsRenderer renderer = WindowContext.Renderer;

        UpdateTextureIfNeeded();
        
        renderer.RendererColor = BackgroundColor;
        renderer.FillRoundedRectangle(ref _bounds, 5);
        
        renderer.RenderTexture(AssignedTexture, _textLocation);
    }

    /// <summary>
    /// Changes Bounds.LOCATION according to given Alignment
    /// IMPORTANT: CHANGES ONLY LOCATION AND MUSTN'T CHANGE ANYTHING ELSE.
    /// </summary>
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

    /// <summary>
    /// Recalculates everything that depends on changing Bounds
    /// </summary>
    public void PerformLocationRecalculation()
    {
        if (AutoSize)
        {
            Bounds = new Rectangle(Bounds.X, Bounds.Y, AssignedTexture.SourceRectangle.Width, AssignedTexture.SourceRectangle.Height);
            _textLocation.X = Bounds.X;
            _textLocation.Y = Bounds.Y;
        }
        else
        {
            _textLocation.X = Bounds.X + Bounds.Width / 2 - AssignedTexture.SourceRectangle.Width / 2;;
            _textLocation.Y = Bounds.Y + Bounds.Height / 2 - AssignedTexture.SourceRectangle.Height / 2;
        }
    }
}