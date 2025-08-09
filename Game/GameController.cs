using JamGame.Graphics;
using JamGame.UI.Fonts;
using SDL2;

namespace JamGame.Game;

/// <summary>
/// Highest architectural level. Behave as it proposed.
/// </summary>
/// 
public class GameController
{
    public GraphicsController _graphicsController;

    public GameController()
    {
        //TODO: MIGHT INITIALIZE EVERYTHING HERE
        
        SDL_ttf.TTF_Init(); //Just let it be here, or make not fucked initializer for it
        FontManager.LoadFonts();
        
        _graphicsController = new GraphicsController();
    }
}