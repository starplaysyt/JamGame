namespace JamGame.Graphics;

/// <summary>
/// Used to register graphics element to rendering pipeline
/// Everything that needs to be rendered should implement that class and  
/// </summary>
public interface IDrawable
{
    public void Draw(GraphicsRenderer renderer);
}