using JamGame.Game.Character;

namespace JamGame.Game.Items;

public class Item : IInventoryble, IArmory
{
    private ItemType _itemType;
    
    private string _name;
    
    private object? _itemObject;
    
    private int _resistance;
    
    private int _strength;

    public Item(ItemType itemType, string name, object? itemObject)
    {
        _itemType = itemType;
        _name = name;
        _itemObject = itemObject;
    }

    public Item(ItemType itemType, string name, object? itemObject, int resistance, int strength)
    {
        _itemType = itemType;
        _name = name;
        _itemObject = itemObject;
        _resistance = resistance;
        _strength = strength;
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
    
    public int Resistance
    {
        get => _resistance;
        set => _resistance = value;
    }

    public int Strength
    {
        get => _strength;
        set => _strength = value;
    }
    
}