using JamGame.Core;
using JamGame.Graphics.Textures;

namespace JamGame.Graphics;

public class Surface : IDrawable
{
    public Texture TileSet { get; set; }
    
    //start rendering point, in pixels
    public Point StartPoint { get; set; } = new Point();
    
    //w and h via pixels, set auto
    public Size PixelSize { get; set; }
    
    //w and h via grid, set in const
    public Size ManualSize { get; set; }
    
    //get zoomfactor from LayerController -> SurfaceLayer
    public double ZoomFactor { get; set; } = 1;
    
    //whole surface texture
    public Texture SurfaceTexture { get; set; }
    
    public int[,] LayerGrid { get; set; }
    
    public Surface(Size manualSize, Texture tileSet, GraphicsRenderer renderer)
    {
        TileSet = tileSet;
        
        ManualSize = manualSize;
        
        SurfaceTexture = new Texture();
        
        PixelSize = new Size(manualSize.Width*32, manualSize.Height*32);
        
        SurfaceTexture.CreateTexture(renderer, TextureType.Target, PixelSize);
        
        var r = new Random();
        
        LayerGrid = new int[manualSize.Width, manualSize.Height];
        for (var i = 0; i < manualSize.Width; i++)
        {
            for (var j = 0; j < manualSize.Height; j++)
            {
                LayerGrid[i, j]= r.Next(0,4);
            }
        }
        
        
    }

    public void Update(GraphicsRenderer renderer)
    {
        Console.WriteLine("Update");
        renderer.RendererTarget = SurfaceTexture;
        renderer.RendererColor = new Color(0, 254, 0, 0);
        renderer.ClearRenderer();

        for (int i = 0; i < ManualSize.Width; i++)
        {
            for (int j = 0; j < ManualSize.Height; j++)
            {
                TileSet.SourceRectangle = new Rectangle(LayerGrid[i, j] % 16 * 32,LayerGrid[i, j] / 16 * 32, 32, 32);
                renderer.RenderTexture(TileSet, new Rectangle(i*32, j*32, 32, 32));
            }
        }

        renderer.RendererTarget = null;
    }
    
    /// <summary>
    /// perform redrawing and resizing by zoom if needed(changing sizes of surfaceTexture when needed
    /// </summary>
    public void Draw(GraphicsRenderer renderer)
    {
        renderer.RenderTexture(SurfaceTexture, new Rectangle(StartPoint.X, StartPoint.Y, (int)(PixelSize.Width * ZoomFactor), (int)(PixelSize.Height * ZoomFactor)));
    }
}