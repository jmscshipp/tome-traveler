using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryUI : InventoryUI
{
    // inherits from InventoryUI, where general inventory functionality comes from
    private void Start()
    {
        // set up reference to inventory
        Player.Instance().PlayerInventory.SetupUIRefs(this);
        UpdateInventory();
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
        UpdateInventory();
    }

    public void UpdateInventory()
    {
        List<Item> itemsShownInUI = new List<Item>();
        List<Item> itemsToRemove = new List<Item>();
        // Remove items in UI but no longer in inventory
        foreach (InventoryItemUI itemui in items)
        {
            Item i = itemui.GetItem();
            if (Player.Instance().PlayerInventory.itemList.Contains(i))
            {
                // remove duplicate InventoryItemUI list entries
                if (itemsShownInUI.Contains(i))
                {
                    itemsToRemove.Add(i);
                }

                itemsShownInUI.Add(i);
            }
            else
            {
                itemsToRemove.Add(i);
            }
            
        }
        
        // We must do this outside of the previous for loop to prevent strange behavior
        foreach (Item i in itemsToRemove)
        {
            Remove(i);
        }

        // Add items in inventory but not shown in UI
        foreach (Item i in Player.Instance().PlayerInventory.itemList)
        {
            if (itemsShownInUI.Contains(i))
            {
                continue;
            }
            else
            {
                Debug.Log("Item not in UI");
                Add(i);
            }
        }
    }
}
