namespace JamGame.Tests;

public static class TestManager
{
    public static void ExecuteCharacterTest()
    {
        Console.WriteLine($"TestManager.ExecuteCharacterTest.TestInventoryAddItem {CharacterTest.TestInventoryAddItem()}");
        Console.WriteLine($"TestManager.ExecuteCharacterTest.TestInventoryRemoveItem {CharacterTest.TestInventoryRemoveItem()}");
        Console.WriteLine($"TestManager.ExecuteCharacterTest.TestInventoryAddItemIfTypeAlreadyExists {CharacterTest.TestInventoryAddItemIfTypeAlreadyExists()}");
        
        Console.WriteLine("Dodge test");
        for(int i =0; i<15; i++)  Console.WriteLine($"TestManager.ExecuteCharacterTest.TestDamageNoProtection {i}: {CharacterTest.TestDamageNoProtection()}");
        Console.WriteLine("Dodge test end");
        
        Console.WriteLine($"TestManager.ExecuteCharacterTest.TestAddProtectionProtection {CharacterTest.TestAddProtectionProtection()}");
        Console.WriteLine($"TestManager.ExecuteCharacterTest.TestRemoveProtectionProtection {CharacterTest.TestRemoveProtectionProtection()}");
        Console.WriteLine($"TestManager.ExecuteCharacterTest.TestDamageWithProtection {CharacterTest.TestDamageWithProtection()}");
        
        
        
    }
}