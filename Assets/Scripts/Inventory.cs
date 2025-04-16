using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}


public class Item
{
    // item ids are read-only
    public Items item_id { get; }
    public Item(Items id)
    {
        item_id = id;
    }
}


public class Tome : Item
{
    public Tome(Items item) : base(item) { }
}

public class TeleportationTome : Tome
{
    public TeleportationTome() : base(Items.TeleportationTome) { }
}
public class SleeplessTome : Tome
{
    public SleeplessTome() : base(Items.SleeplessTome) { }
}
public class AbundanceTome : Tome
{
    public AbundanceTome() : base(Items.AbundanceTome) { }
}
public class ClairvoyanceTome : Tome
{
    public ClairvoyanceTome() : base(Items.ClairvoyanceTome) { }
}
public class MindreadingTome : Tome
{
    public MindreadingTome() : base(Items.MindreadingTome) { }
}

public class Food : Item
{
    public Food() : base(Items.Food) { }
}

public class Treasure : Item
{
    public Treasure() : base(Items.Treasure) { }
}

public class Tent : Item
{
    public int Uses { get; set; }

    public bool UseTent()
    // returns true if tent breaks
    {
        Uses -= 1;
        return Uses < 0;
    }
    public Tent() : base(Items.Tent) {}
}

public class Inventory
{
    public List<Item> itemList = new List<Item>();

    public void AddItem(Item item)
    {
        itemList.Add(item);
    }

    public bool HasItem(Items ItemID)
    // check the inventory has an item
    {
        return itemList.Exists(x => x.item_id == ItemID);
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

    public bool RemoveItem(Items ItemID)
    // Returns true if item successfully removed
    {
        foreach (Item i in itemList)
        {
            if (i.item_id == ItemID)
            {
                itemList.Remove(i);
                return true;
            }
        }
        return false;
    }
    public Inventory() { }
}
