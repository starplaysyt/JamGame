using JamGame.Game.Items;

namespace JamGame.Game.Character;

public class Character
{
    private Statistic _statistic;
    private Inventory _inventory;
    private CharacterPosition _position;

    public Character()
    {
        _statistic = new Statistic();
        _inventory = new Inventory();
        _position = new CharacterPosition();
    }

    public bool GetDamage(int val) => _statistic.GetDamage(val);

    public TakeResult TakeItem(Item item)
    {
        _statistic.Resistance += item.Resistance;

        return _inventory.TakeItem(item);
    }

    public IInventoryble DropItem(string name)
    {
        var removed = _inventory.DropItem(name);
        _statistic.Resistance -= (removed as Item).Resistance;
        return removed;
    }

    public int Resistance => _statistic.Resistance;

    public Inventory Inventory => _inventory;
    
    public int Health => _statistic.Health;
}