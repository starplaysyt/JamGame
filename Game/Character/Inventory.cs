namespace JamGame.Game.Character;

using System.Linq;

public class Inventory
{
    
    private List<IInventoryble> _items;

    public Inventory()
    {
        _items = new List<IInventoryble>();
    }
    
    public TakeResult TakeItem(IInventoryble item)
    {
        foreach (var i in _items)
        {
            if (item.CurrentItemType == i.CurrentItemType)
            {
                return TakeResult.NoPlaceInInventory;
            }
        }
        _items.Add(item);
        return TakeResult.Success;
    }

    public List<IInventoryble> Items => _items;

    public IInventoryble DropItem(string name)
    {
        var res = _items.FirstOrDefault(el => el.Description == name);
        if (res != null)
        {
            _items.Remove(res);
            return res;
        }

        return null;
    }
}