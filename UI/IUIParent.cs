using JamGame.Core;

namespace JamGame.UI;

public interface IUIParent
{
    /// <summary>
    /// This should return base parent location and its size to perform alignment operations
    /// if needed
    /// </summary>
    /// <returns></returns>
    public Rectangle GetParentParameters();
}