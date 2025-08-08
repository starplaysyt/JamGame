using JamGame.Game.Character;
using JamGame.Game.Items;

namespace JamGame.Tests;

public static class CharacterTest
{
   
    public static bool TestInventoryAddItem() // success
    {
      
        var character = new Character();
    
        var item = new Item(ItemType.Body, "testItem", null);
        
        character.Inventory.TakeItem(item);
        return character.Inventory.Items.Contains(item);
    }

    public static bool TestInventoryRemoveItem() // success
    {
        var character = new Character();
        var item = new Item(ItemType.Body, "testItem", null);
        character.Inventory.TakeItem(item);
        return item == character.Inventory.DropItem("testItem") && !character.Inventory.Items.Contains(item);;
    }
    
}