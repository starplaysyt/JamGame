namespace JamGame.Game.Character;

public class InventoryItem:IInventoryble
{
    
    private string _name;

    private object? _itemObject;
    
    public TakeResult Take()
    {
        return TakeResult.Success;
    }

    public object ItemObject
    {
        get => _itemObject;
        set => _itemObject = value;
    }

    public string Description => _name;
}