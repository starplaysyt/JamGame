namespace JamGame.Graphics;

[Flags]
public enum FCameraFlowDirection
{
    None = 0,
    Upwards = 1 << 0, 
    Downwards = 1 << 1,
    Leftwards = 1 << 2,
    Rightwards =  1 << 3,
}