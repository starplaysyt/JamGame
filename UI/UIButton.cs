using JamGame.Core;
using JamGame.Graphics;
using JamGame.Graphics.Textures;
using JamGame.UI.Fonts;
using SDL2;

namespace JamGame.UI;

public class UIButton : IControl
{
    private bool _needUpdate = false;
    private string _text;
    private Rectangle _shadowRectangle;
    private Rectangle _strokeRectangle;
    private Rectangle _bounds;
    private Rectangle _textLocation;

    private Rectangle _borderRect;
    public bool IsVisible { get; set; }
    public bool IsEnabled { get; set; }
    public bool IsMouseInside { get; set; } = false;
    
    public bool IsClicked { get; set; } = false;

    public event EventHandler MouseClick;
    
    public Texture AssignedTexture { get; set; } = new Texture();
    
    public Font AssignedFont { get; set; }
    
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
            _shadowRectangle = new Rectangle(value.X + 2, value.Y + 2, value.Width, value.Height);
            _strokeRectangle = new Rectangle(value.X - 3, value.Y - 3, value.Width, value.Height);
            _bounds = new Rectangle(value.X, value.Y, value.Width - 6, value.Height - 6);
        }
    }

    public UIButton()
    {
        IsVisible = true;
        IsEnabled = false;
        IsMouseInside = false;
    }
    public bool CheckMouseInside(Point mousePos)
    {
        return mousePos.X > Bounds.X && mousePos.X < Bounds.X + Bounds.Width && mousePos.Y > Bounds.Y && mousePos.Y < Bounds.Y + Bounds.Height;
    }

    public bool ProcessEvents(ref SDL.SDL_Event e)
    {
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

    public void Draw()
    {
        GraphicsRenderer renderer = WindowContext.Renderer;
        if (_needUpdate || AssignedTexture.IsAssigned == false)
        {
            renderer.RendererColor = new Color(0, 0, 0);
            AssignedFont.RenderFont(AssignedTexture, TextSize, Text);
            
            _textLocation.X = Bounds.X + Bounds.Width / 2 - AssignedTexture.SourceRectangle.Width / 2;;
            _textLocation.Y = Bounds.Y + Bounds.Height / 2 - AssignedTexture.SourceRectangle.Height / 2;
            
            _textLocation.Width = AssignedTexture.SourceRectangle.Width;
            _textLocation.Height = AssignedTexture.SourceRectangle.Height;
            _needUpdate = false;
        }
        
        
        

        
        renderer.RendererColor = new Color(102, 57, 49);
        renderer.FillRoundedRectangle(ref _shadowRectangle, 5);
        renderer.RendererColor = new Color(0, 0, 0);
        renderer.FillRoundedRectangle(ref _strokeRectangle, 5);
        
        if (IsClicked)
            renderer.RendererColor = new Color(168, 137, 107);
        else
            renderer.RendererColor = new Color(238, 195, 154);
        renderer.FillRoundedRectangle(ref _bounds, 5);


        
        renderer.RenderTexture(AssignedTexture, _textLocation);
    }
}