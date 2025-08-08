using JamGame.Game.Character;

namespace JamGame.Game.Items;

public class Item : IInventoryble
{
    private ItemType _itemType;
    
    private string _name;
    
    private object? _itemObject;

    public Item(ItemType itemType, string name, object? itemObject)
    {
        _itemType = itemType;
        _name = name;
        _itemObject = itemObject;
    }

    public Item()
    {
        _name = "";
    }
    
    
    public ItemType CurrentItemType => _itemType;
    
    public object ItemObject
    {
        get => _itemObject;
        set => _itemObject = value;
    }
    
    public string Description => _name;

    
}