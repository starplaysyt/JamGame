using SDL2;

namespace JamGame.Graphics;

/// <summary>
/// Layer behavior declaration. LayerController will pass through
/// </summary>
public interface ILayer
{
    public string Name { get; set; }
    public bool IsVisible { get; set; }
    public void Draw();

    /// <summary>
    /// Process event on specific layer, and gets it when that all is 
    /// </summary>
    /// <param name="e"></param>
    /// <returns>Whether pipeline need to pass that event lower, or not</returns>
    public bool Update(ref SDL.SDL_Event e);

    public void RegisterGraphics(IDrawable drawable);
}