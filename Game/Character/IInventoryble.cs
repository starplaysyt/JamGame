using JamGame.Game.Items;

namespace JamGame.Game.Character;

public interface IInventoryble
{
    string Description { get; }
    public ItemType CurrentItemType {get; }
}