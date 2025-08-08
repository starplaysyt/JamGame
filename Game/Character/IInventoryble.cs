namespace JamGame.Game.Character;

public interface IInventoryble
{
    TakeResult Take();
    string Description { get; }
}