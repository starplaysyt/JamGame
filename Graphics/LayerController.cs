using JamGame.Graphics.Layers;
using SDL2;

namespace JamGame.Graphics;

/// <summary>
/// General home for all layers
/// </summary>
public class LayerController
{
    public List<ILayer> PipelineLayers = new List<ILayer>();

    public LayerController()
    {
        PipelineLayers = new List<ILayer>() {
            //UI Layer
            //Effects Layer
            //Characters Layer
            //Game objects Layer
            //Surface Effects Layer
            new SurfaceLayer()
        };
        //TODO: Add stuff to LayerControllers' constructor
    }

    public void UpdateLayers(ref SDL.SDL_Event e)
    {
        foreach (var layer in PipelineLayers)
        {
            if (layer.Update(ref e) == false)
                break;
        }
    }

    public void DrawLayers()
    {
        for (int i = PipelineLayers.Count - 1; i >= 0; i--)
        {
            if (PipelineLayers[i].IsVisible) 
                PipelineLayers[i].Draw();
        }
    }

    public void AddObjectToLayer(IDrawable drawable, string layerName)
    {
        PipelineLayers.Find(layer => layer.Name == layerName)?.RegisterGraphics(drawable);
    }
}