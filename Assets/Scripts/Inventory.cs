using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ShopActions
{
    None,
    Buying,
    Selling
}

public enum Items : ushort
{
    None = 0,
    Food,
    Tent,
    Treasure,
    TeleportationTome,
    SleeplessTome,
    AbundanceTome,
    ClairvoyanceTome,
    MindreadingTome,
    WaterwalkingTome,
}

public abstract class Item
{
    public int cost = 69;

    int index = -1;

    // item ids are read-only
    public Items item_id { get; }
    protected Item(Items id)
    {
        item_id = id;
    }
    internal void SetIndex(int i)
    {
        index = i;
    }
    internal int GetIndex()
    {
        return index;
    }

    bool an = false;
    public abstract string Name { get; }

    public string IndefiniteArticle()
    {
        if (an)
            return "an " + Name;
        return Name;
    }
}

public class Tome : Item
{
    public Spell Spell;

    public override string Name => throw new System.NotImplementedException();

    public Tome(Spell spell) : base(spell.ItemId)
    {
        Spell = spell;
    }
}

public class TeleportationTome : Tome
{
    public TeleportationTome() : base(Player.teleportation) { }
}
public class SleeplessTome : Tome
{
    public SleeplessTome() : base(Player.sleepless) { }
}
public class AbundanceTome : Tome
{
    public AbundanceTome() : base(Player.abundance) { }
}
public class ClairvoyanceTome : Tome
{
    public ClairvoyanceTome() : base(Player.clairvoyance) { }
}
public class MindreadingTome : Tome
{
    public MindreadingTome() : base(Player.mindreading) { }
}

public class WaterwalkingTome : Tome
{
    public WaterwalkingTome() : base(Player.waterwalking) { }
}

public abstract class Usable : Item
{
    public int Uses { get; set; }

    public Usable(Items itemid) : base(itemid) {}
    
    public bool Use()
    {
        Uses -= 1;
        return Uses < 0;
    }
}

public class Food : Usable
{
    public Food() : base(Items.Food) { }

    public override string Name => "food";

}
public class Tent : Usable
{
    public Tent() : base(Items.Tent)
    {
        Uses = Random.Range(2, 6);
    }

    public override string Name => "tent";

}

public class Treasure : Item
{
    public Treasure() : base(Items.Treasure) { }

    public override string Name => "treasure";

}

public class Inventory
{
    public List<Item> itemList = new List<Item>();
    private PlayerInventoryUI ui;

    public void SetupUIRefs(PlayerInventoryUI uiRef)
    {
        ui = uiRef;
    }

    public int SizeLimit = 0;

    public void LogInventory(string prompt)
    {
        string itemstring = prompt + ": ";
        foreach (Item i in itemList)
            itemstring += i.ToString();
        Debug.Log(itemstring);
    }

    public static Item NewItemById(Items id)
    {
        switch (id)
        {
            case Items.Food:
                return new Food();
            case Items.Tent:
                return new Tent();
            case Items.Treasure:
                return new Treasure();
            case Items.AbundanceTome:
                return new AbundanceTome();
            case Items.TeleportationTome:
                return new TeleportationTome();
            case Items.MindreadingTome:
                return new MindreadingTome();
            case Items.WaterwalkingTome:
                return new WaterwalkingTome();
            case Items.SleeplessTome:
                return new SleeplessTome();
            case Items.ClairvoyanceTome:
                return new ClairvoyanceTome();
            default:
                Debug.LogError("Item id not implemented");
                return null; 
        }
    }


    public void UpdateItemIndices()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            itemList[i].SetIndex(i);
        }
    }

    public void AddItem(Item item)
    {
        //LogInventory("Items Before");

        itemList.Add(item);
        UpdateItemIndices();
        ui.UpdateInventory();
        //LogInventory("Items After");
    }

    public bool HasItem(Items ItemID)
    // check the inventory has an item
    {
        return itemList.Exists(x => x.item_id == ItemID);
    }

    public bool HasSpace()
    {
        return itemList.Count < SizeLimit;
    }

    public Tent GetUsableTent()
    {
        foreach (Item i in itemList)
        {
            if (i.item_id != Items.Tent)
            {
                continue;
            }
            Tent t = (Tent)i;
            if (t.Uses <0) {
                continue;
            }
            return t;
        }
        Debug.LogWarning("GetUsableTent failed: No tent with uses remaining");
        return null;
    }

    public Food GetEdibleFood()
    {
        foreach (Item i in itemList)
        {
            if (i.item_id != Items.Food)
            {
                continue;
            }
            Food t = (Food)i;
            if (t.Uses <0) {
                continue;
            }
            return t;
        }
        Debug.LogWarning("GetEdibleFood failed: No tent with uses remaining");
        return null;
    }

    public bool RemoveItem(Items ItemID)
    // Returns true if item successfully removed
    {
        foreach (Item i in itemList)
        {
            if (i.item_id == ItemID)
            {
                itemList.Remove(i);
                UpdateItemIndices();
                ui.UpdateInventory();
                return true;
            }
        }
        return false;
    }

    public bool RemoveItem(Item item)
    // Returns true if item successfully removed
    {
        if (itemList.Contains(item))
        {
            itemList.Remove(item);
            UpdateItemIndices();
            ui.UpdateInventory();
            return true;
        }
        return false;
    }

    public PlayerInventoryUI GetInventoryUI() => ui;
    public Inventory() { }
}
