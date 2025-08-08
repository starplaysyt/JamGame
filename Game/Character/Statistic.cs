namespace JamGame.Game.Character;

public class Statistic
{
    private int _health;

    private int _resistance;

    public Statistic()
    {
        _health = 100;
        _resistance = 0;
    }
    
    
    public int Health
    {
        get => _health;
        set => _health = value;
    }

    public bool GetDamage(int damage) // bool use as attack result: true - success, false - fail
    {
        var chance = new Random().Next(1, 11);
        if (chance == 4) return false;

        _health = _health - damage * (_resistance / 100);
        return true;
    }
}