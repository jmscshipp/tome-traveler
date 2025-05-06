using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerInventoryUI : InventoryUI
{
    public bool InShop = false;
    public bool Learning = false;
    public ShopInventoryUI ShopUI;

    // inherits from InventoryUI, where general inventory functionality comes from
    private void Start()
    {
        SetupUIRefs();
        UpdateInventory();
    }

    protected virtual void SetupUIRefs()
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
    
    public void MakeTomesClickable(bool clickable)
    {
        InventoryItemUI[] items = inventoryObjParent.GetComponentsInChildren<InventoryItemUI>();
        foreach (InventoryItemUI itemUI in items)
        {
            if (itemUI.GetItem() is Tome tome)
            {
                Learning |= clickable;
                itemUI.SetLearnable(clickable);
            }
        }
    }

    internal override void LearnItem(InventoryItemUI inventoryItemUI)
    {
        if (inventoryItemUI.GetItem() is Tome tome)
        {
            Remove(inventoryItemUI);
            Player.Instance().Learn(tome);
            UpdateInventory();
            Learning = false;
        }
        else
        {
            Debug.LogWarning("Can't read item, it's not a tome!", inventoryItemUI);
        }
    }

    public void SetShopUI(InventoryUI ui)
    {
        foreach (InventoryItemUI uItem in inventoryObjParent.GetComponentsInChildren<InventoryItemUI>())
        {
            uItem.inventoryUI = this;
        }
    }

    public override void SellItem(InventoryItemUI item)
    {
        //Debug.Log("Player is selling item");
        // add coins to player inventory
        Player.Instance().resources.AddCoins(item.GetItemCost());
        // remove Item from Player inventory
        Player.Instance().PlayerInventory.RemoveItem(item.GetItem());
        // remove item from inventory menu
        Remove(item);
        UpdateInventory();
        ShopUI.RefreshShopAvailability();
    }

    protected override List<Item> GetItems()
    {
        return Player.Instance().PlayerInventory.itemList;
    }
    public void UpdateInventory()
    {
        Clear();
        foreach (Item i in GetItems())
        {
            Add(i);
        }
        if (InShop)
        {
            SetPrices(ShopActions.Selling);
            MakeInventoryClickable(true);
        }
        else
        {
            SetPrices(ShopActions.None);
            MakeInventoryClickable(false);
        }
    }
}
