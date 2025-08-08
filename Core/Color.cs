using SDL2;

namespace JamGame.Core;

public struct Color(byte r, byte g, byte b, byte a = 255) : IEquatable<Color>
{
    public byte R { get; private set; } = r;
    public byte G { get; private set; } = g;
    public byte B { get; private set; } = b;
    public byte A { get; private set; } = a;

    public Color() : this(0, 0, 0) { }

    internal SDL.SDL_Color ToSDL() => new() { r = R, g = G, b = B, a = A };

    internal void Set(SDL.SDL_Color color)
    {
        R = color.r; G = color.g; B = color.b; A = color.a;
    }
    
    public void Set(byte r, byte g, byte b, byte a = 255)
    {
        R = r; G = g; B = b; A = a;
    }
    
    public static bool operator!= (Color a, Color b) => !(a == b);

    public static bool operator== (Color a, Color b) => a.Equals(b);

    public bool Equals(Color other) => R == other.R && G == other.G && B == other.B && A == other.A;

    public override bool Equals(object? obj) => obj is Color other && Equals(other);

    public override int GetHashCode()
    {
        return HashCode.Combine(R.GetHashCode(), G.GetHashCode(), B.GetHashCode(), A.GetHashCode());
    }

    public override string ToString() => $"R: {R}, G: {G}, B: {B}, A: {A}";
}