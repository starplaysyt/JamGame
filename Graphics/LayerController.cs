using JamGame.Core;
using JamGame.Graphics.Layers;
using SDL2;

namespace JamGame.Graphics;

/// <summary>
/// General home for all layers
/// </summary>
public class LayerController
{
    public Point PixelLocation { get; set; }
    
    public double LayerZoom { get; set; }
    
    public List<ILayer> PipelineLayers = new List<ILayer>();

    public SurfaceLayer SurfaceLayer = new();

    public PlayerCamera PlayerCamera = new();

    public LayerController()
    {
        PipelineLayers = new List<ILayer>() {
            //UI Layer
            PlayerCamera,
            //Effects Layer
            //Characters Layer
            //Game objects Layer
            //Surface Effects Layer
            SurfaceLayer
        };
        //TODO: Add stuff to LayerControllers' constructor
    }
    
    public void Update(ref SDL.SDL_Event e)
    {
        foreach (var layer in PipelineLayers)
        {
            var res = layer.UpdateEvents(ref e);
            layer.GetController(this);
            if (res == false)
                break;
        }
    }

    public void UpdateByTick()
    {
        foreach (var layer in PipelineLayers)
        {
            layer.UpdateByTick();
            layer.GetController(this);
        }
    }

    public void DrawLayers()
    {
        for (int i = PipelineLayers.Count - 1; i >= 0; i--)
        {
            if (PipelineLayers[i].IsVisible)
            {
                PipelineLayers[i].GetController(this); //TODO: This shouldn't be here, but without that 
                PipelineLayers[i].Draw();
            }
                
        }
    }

    public void AddObjectToLayer(IDrawable drawable, string layerName)
    {
        PipelineLayers.Find(layer => layer.Name == layerName)?.RegisterGraphics(drawable);
    }
}