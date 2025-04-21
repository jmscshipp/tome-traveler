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
