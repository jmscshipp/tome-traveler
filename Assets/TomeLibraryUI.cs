using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomeLibraryUI : PlayerInventoryUI
{
    public override void SellItem(InventoryItemUI item)
    {
        // add coins to player inventory
        Player.Instance().resources.AddCoins(item.GetItemCost());
        // remove Item from Player inventory
        Player.Instance().PlayerInventory.RemoveItem(item.GetItem());
        // remove item from inventory menu
        Remove(item);
        UpdateInventory();
        MakeInventoryClickable(true);
        ShopUI.RefreshShopAvailability();
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
