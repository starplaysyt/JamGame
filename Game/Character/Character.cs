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


    
    public Inventory Inventory => _inventory;
    
}