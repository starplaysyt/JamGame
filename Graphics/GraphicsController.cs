using JamGame.Core;

namespace JamGame.Graphics;

/// <summary>
/// Highest graphics architectural level. Everything what we do with graphics should be here.
/// Home of graphics layers
/// </summary>

public class GraphicsController
{
    public LayerController LayerController = new LayerController();
    
    public Point LayerLocation { get; set; }
    
    public double LayerZoom { get; set; }

    public GraphicsController()
    {
        
    }
}