namespace JamGame.Graphics.Textures;

public enum TextureType
{
    /// <summary>
    /// Inaccessible for changing after creation. Good choice for static pictures and other stuff like that.
    /// </summary>
    Static, 
    /// <summary>
    /// Changeable, gives direct access to pixels. Good choice for creating dynamic effects.
    /// <remarks>Stores array in RAM, and updates it</remarks>
    /// </summary>
    Streaming, 
    /// <summary>
    /// Changeable, gives ability to render on this texture primitives.
    /// <remarks>Stores graphics on GPU</remarks>
    /// </summary>
    Target
}