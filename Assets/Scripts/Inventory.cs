using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class Item
{
    public int cost;
    // item ids are read-only
    public Items item_id { get; }
    public Item(Items id)
    {
        item_id = id;
    }
}

public class Tome : Item
{
    public Spell Spell;
    public Tome(Items item, Spell spell) : base(item) {
        Spell = spell;
    }
}

public class TeleportationTome : Tome
{
    public TeleportationTome() : base(Items.TeleportationTome, new Teleportation()) { }
}
public class SleeplessTome : Tome
{
    public SleeplessTome() : base(Items.SleeplessTome, new Sleepless()) { }
}
public class AbundanceTome : Tome
{
    public AbundanceTome() : base(Items.AbundanceTome, new Abundance()) { }
}
public class ClairvoyanceTome : Tome
{
    public ClairvoyanceTome() : base(Items.ClairvoyanceTome, new Clairvoyance()) { }
}
public class MindreadingTome : Tome
{
    public MindreadingTome() : base(Items.MindreadingTome, new Mindreading()) { }
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
}
public class Tent : Usable
{
    public Tent() : base(Items.Tent) {}
}

public class Treasure : Item
{
    public Treasure() : base(Items.Treasure) { }
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

    public void AddItem(Item item)
    {
        itemList.Add(item);
        ui.Add(item, ShopActions.Selling);
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
                ui.Remove(i);
                itemList.Remove(i);
                return true;
            }
        }
        return false;
    }

    public PlayerInventoryUI GetInventoryUI() => ui;
    public Inventory() { }
}
