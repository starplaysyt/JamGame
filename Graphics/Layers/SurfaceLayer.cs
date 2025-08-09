using JamGame.Core;
using JamGame.Graphics.Textures;
using SDL2;

namespace JamGame.Graphics.Layers;

public class SurfaceLayer : ILayer
{
    //Start rendering point, fully final, in pixels
    public Point PixelLocation { get; set; }
    public string Name { get; set; }
    public bool IsVisible { get; set; } = true;
    
    public double LayerZoom { get; set; }
    
    public Surface CurrentSurface { get; set; }
    
    public Texture SurfaceTileSet { get; set; }

    public SurfaceLayer()
    {
        SurfaceTileSet = new Texture();
        IntPtr tilesetTexture = SDL2.SDL_image.IMG_LoadTexture(WindowContext.Renderer.SDLRenderer,"Assets/gameSurfaceTextures.png");
        Console.WriteLine(SDL_image.IMG_GetError());
        SurfaceTileSet.AssignTexture(tilesetTexture, TextureType.Static, new Rectangle(0,0, 160, 32));
        CurrentSurface = new Surface(new Size(100, 100), SurfaceTileSet, WindowContext.Renderer);
        CurrentSurface.Update(WindowContext.Renderer);
    }
    
    public void Draw()
    {
        //WindowContext.Renderer.RenderTexture(SurfaceTileSet, new Rectangle(0,0, 160, 32));
        CurrentSurface.Draw(WindowContext.Renderer);
    }

    public void GetController(LayerController controller)
    {
        LayerZoom = controller.LayerZoom;
        PixelLocation = controller.PixelLocation;
    }

    public void UpdateByTick()
    {
        CurrentSurface.StartPoint = PixelLocation;
        CurrentSurface.ZoomFactor = LayerZoom;
    }

    public bool UpdateEvents(ref SDL.SDL_Event e)
    {

        return true;
    }

    public void RegisterGraphics(IDrawable drawable)
    {
        throw new NotImplementedException();
    }
}