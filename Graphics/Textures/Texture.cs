using JamGame.Core;
using SDL2;
using TDK.Logger;

namespace JamGame.Graphics.Textures;

public class Texture
{
    /// <summary>
    /// Contains self SDL texture
    /// </summary>
    public IntPtr SDLTexture { get; private set; } 
    
    /// <summary>
    /// Contains textures' actual parameters, as they is
    /// </summary>
    public Rectangle TextureParameters { get; private set; }
    
    /// <summary>
    /// Contains assigned texture parameters
    /// </summary>
    public Rectangle SourceRectangle { get; set; } 
    
    public TextureType TextureType { get; private set; }

    public bool IsAssigned = false;

    public void AssignTexture(IntPtr sdlTexture, TextureType type, Rectangle textureParameters)
    {
        if (IsAssigned)
        {
            ConsoleLogger.LogError("Texture",$"Texture already assigned. Clear previous texture before connecting a new one.");
            return;
        }
        
        TextureType = type;
        SDLTexture = sdlTexture;
        TextureParameters = textureParameters;
        SourceRectangle = textureParameters;
        IsAssigned = true;
    }

    public void CreateTexture(GraphicsRenderer renderer, TextureType type, Size textureSize)
    {
        if (IsAssigned) {
            ConsoleLogger.LogError("Texture", "Texture already assigned. Clear previous texture before connecting a new one.");
            return;
        }
        
        SDLTexture = SDL.SDL_CreateTexture(renderer.SDLRenderer, 0,
            (int)type, textureSize.Width, textureSize.Height);
        
        TextureParameters = new Rectangle(0, 0, textureSize.Width, textureSize.Height);
        SourceRectangle = TextureParameters;

        if (SDLTexture == IntPtr.Zero)
        {
            ConsoleLogger.LogFatal("Texture", $"Failed to create texture. More info: {SDL.SDL_GetError()}");
            return;
        }
        
        IsAssigned = true;
    }

    public void DeleteTexture()
    {
        if (!IsAssigned)
            return;
        
        SDL.SDL_DestroyTexture(SDLTexture);
        
        IsAssigned = false;
    }
}