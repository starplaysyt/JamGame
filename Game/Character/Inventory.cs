namespace JamGame.Game.Character;
using System.Linq;


public class Inventory
{ 
    private List<IInventoryble> _items = new List<IInventoryble>();
    
    public void TakeItem(IInventoryble item)
    {
        _items.Add(item);
    }

    public IInventoryble DropItem(string name)
    {
        var res = _items.FirstOrDefault(el=>el.Description==name);
        if (res != null)
        {
            _items.Remove(res);
            return res;
        }
        return null;
    }
    
}