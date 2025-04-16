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
    public Tent() : base(Items.Tent)
    {
    }
}

public class Inventory
{
    List<Item> itemList = new List<Item>();

    public void AddItem(Item item)
    {
        itemList.Add(item);
    }

    public bool HasItem(Items ItemID)
    // check the inventory has an item
    {
        return itemList.Exists(x => x.item_id == ItemID);
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
