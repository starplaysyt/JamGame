using JamGame.Game.Character;
using JamGame.Game.Items;

namespace JamGame.Tests;

public static class CharacterTest
{
    public static bool TestInventoryAddItem() // success
    {
        var character = new Character();

        var item = new Item(ItemType.Body, "testItem", null);

        character.TakeItem(item);
        return character.Inventory.Items.Contains(item);
    }

    public static bool TestInventoryRemoveItem() // success
    {
        var character = new Character();
        var item = new Item(ItemType.Body, "testItem", null);
        character.TakeItem(item);
        return item == character.Inventory.DropItem("testItem") && !character.Inventory.Items.Contains(item);
    }

    public static bool TestInventoryAddItemIfTypeAlreadyExists() // success
    {
        var character = new Character();
        var item = new Item(ItemType.Body, "testItem", null);
        character.TakeItem(item);
        return character.TakeItem(item) == TakeResult.NoPlaceInInventory;
    }

    public static bool TestDamageNoProtection()
    {
        var character = new Character();
        return character.GetDamage(100);
    }

    public static bool TestAddProtectionProtection()
    {
        var character = new Character();
        var  item = new Item(ItemType.Body, "testItem", null, 10, 100);
        
        character.TakeItem(item);

        return character.Resistance == 10;

    }
}