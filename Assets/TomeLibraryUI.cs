using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomeLibraryUI : PlayerInventoryUI
{
    public override void SellItem(InventoryItemUI item)
    {
        // You can't sell learned tomes
    }

    protected override void SetupUIRefs()
    {
        // set up reference to inventory
        Player.Instance().PlayerInventory.tomeUI = this;
    }
    
    public new void Add(Item item)
    {
        base.Add(item);
    }
    public new void Remove(Item item)
    {
        base.Remove(item);
    }
    protected override List<Item> GetItems()
    {
        return Player.Instance().PlayerInventory.Tomes;
    }
}
