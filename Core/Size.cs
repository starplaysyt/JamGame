namespace JamGame.Core;

public struct Size(int width, int height) : IEquatable<Size>
{
    public int Width { get; set; } = width;
    public int Height { get; set; } = height;

    public Size() : this(0, 0) { }

    public void Set(int width, int height) { Width = width; Height = height; }
    
    public static bool operator !=(Size left, Size right) => !(left == right);
    public static bool operator ==(Size left, Size right) => left.Width == right.Width && left.Height == right.Height;

    public override bool Equals(object? obj) => obj is Size size && size.Width == Width && size.Height == Height;

    public bool Equals(Size other) => Width == other.Width && Height == other.Height;

    public override int GetHashCode()
    {
        return HashCode.Combine(Width, Height);
    }

    public override string ToString() => $"{Width}x{Height}";
}