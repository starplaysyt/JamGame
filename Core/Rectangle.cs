using SDL2;

namespace JamGame.Core;

public struct Rectangle(int x, int y, int width, int height)
{
    public int X { get; private set; } = x;
    
    public int Y { get; private set; } = y;
    
    public int Width { get; set; } = width;
    
    public int Height { get; set; } = height;
    
    public Point Location
    {
        get => new(X, Y);
        set
        {
            X = value.X;
            Y = value.Y;
        }
    }

    public Size Size
    {
        get => new(Width, Height);
        set
        {
            Width = value.Width;
            Height = value.Height;
        }
    }
    
    public Rectangle(Point location, Size size) : this(location.X, location.Y, size.Width, size.Height) { }

    public Rectangle() : this(0, 0, 0, 0) { }

    internal void Set(SDL.SDL_Rect rect)
    {
        X = rect.x;
        Y = rect.y;
        Width = rect.w;
        Height = rect.h;
    }

    internal SDL.SDL_Rect ToSDL() => new SDL.SDL_Rect() {x = X, y = Y, h = Height, w = Width };

    public void Set(int x, int y, int width, int height)
    {
        X = x; Y = y;
        Width = width; Height = height;
    }

    internal void SetLoc(int x, int y)
    {
        X = x; Y = y;
    }

    internal void SetSize(int w, int h)
    {
        Width = w; Height = h;
    }
    
    public override string ToString() => $"({X}, {Y}, {Width}, {Height})";
}