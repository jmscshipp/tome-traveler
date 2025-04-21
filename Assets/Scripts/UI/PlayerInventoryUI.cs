using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryUI : InventoryUI
{
    // inherits from InventoryUI, where general inventory functionality comes from
    private void Start()
    {
        // set up reference to inventory
        Player.Instance().PlayerInventory.SetupUIRefs(this);
    }

    // called when a shop is opened to allow player to sell out of their inventory
    public void MakeInventoryClickable(bool clickable)
    {
        foreach (InventoryItemUI itemUI in inventoryObjParent.GetComponentsInChildren<InventoryItemUI>())
            itemUI.SetSellable(clickable);
    }

    public override void SellItem(InventoryItemUI item)
    {
        // add coins to player inventory
        Player.Instance().GetPlayerResources().AddCoins(item.GetItemCost());
        // remove Item from Player inventory
        Player.Instance().PlayerInventory.RemoveItem(item.GetItem().item_id);
        Remove(item.GetItem());
    }

    public override void AssignInventoryUI(InventoryItemUI itemUI)
    {
        itemUI.AssignInventoryUI(this);
    }
}
