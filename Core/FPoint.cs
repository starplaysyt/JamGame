using SDL2;

namespace JamGame.Core;

public struct FPoint(float x, float y) : IEquatable<FPoint>
{
    public float X { get; private set; } = x;
    public float Y { get; private set; } = y;

    public FPoint() : this(0, 0) { }
    
    internal SDL.SDL_Point ToSDL() => new() { x = (int)X, y = (int)Y };
    
    public void Set(int x, int y) { X = x; Y = y; }
    
    internal void Set(SDL.SDL_Point point) { X = point.x; Y = point.y; }
    
    public static bool operator != (FPoint left, FPoint right) => !(left == right);
    public static bool operator == (FPoint left, FPoint right) => left.X == right.X && left.Y == right.Y;

    public bool Equals(FPoint other) => X == other.X && Y == other.Y;
    
    public Point ToPoint() => new((int)X, (int)Y);
    
    public override bool Equals(object? obj) => obj is Point point && Equals(point);
    
    public override int GetHashCode() => HashCode.Combine(X.GetHashCode(), Y.GetHashCode());

    public override string ToString() => $"({X}, {Y})";
}