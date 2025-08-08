namespace JamGame.Tests;

public static class TestManager
{
    public static void ExecuteCharacterTest()
    {
        Console.WriteLine($"TestManager.ExecuteCharacterTest.TestInventoryAddItem {CharacterTest.TestInventoryAddItem()}");
        Console.WriteLine($"TestManager.ExecuteCharacterTest.TestInventoryRemoveItem {CharacterTest.TestInventoryRemoveItem()}");
    }
}