using SDL2;

namespace JamGame.Core;

public struct Point(int x, int y) : IEquatable<Point>
{
    public int X { get; private set; } = x;
    public int Y { get; private set; } = y;

    public Point() : this(0, 0) { }
    
    internal SDL.SDL_Point ToSDL() => new() { x = X, y = Y };
    
    public void Set(int x, int y) { X = x; Y = y; }
    
    internal void Set(SDL.SDL_Point point) { X = point.x; Y = point.y; }
    
    public static bool operator !=(Point left, Point right) => !(left == right);
    public static bool operator == (Point left, Point right) => left.X == right.X && left.Y == right.Y;

    public bool Equals(Point other) => X == other.X && Y == other.Y;
    
    public override bool Equals(object? obj) => obj is Point point && Equals(point);
    
    public override int GetHashCode() => HashCode.Combine(X.GetHashCode(), Y.GetHashCode());

    public override string ToString() => $"({X}, {Y})";
}