using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInventoryUI : InventoryUI
{
    // inherits from InventoryUI, where general inventory functionality comes from

    // called by shops when player selects 'buy / sell'
    public void SetupShop(Shop shopLocation)
    {
        foreach(ShopItem shopItem in shopLocation.GetShopItems())
        {
            for (int i = 0; i < shopItem.quantity; i++)
                Add(Inventory.NewItemById(shopItem.itemType));
        }
        FinalizeSetup();
    }

    // overriden version for cities to use
    public void SetupShop(City cityLocation)
    {
        foreach (ShopItem shopItem in cityLocation.GetShopItems())
        {
            for (int i = 0; i < shopItem.quantity; i++)
                Add(Inventory.NewItemById(shopItem.itemType));
        }
        FinalizeSetup();
    }

    private void FinalizeSetup()
    {
        Player.Instance().PlayerInventory.GetInventoryUI().InShop = true;
        Player.Instance().PlayerInventory.GetInventoryUI().ShopUI = this;
        SetPrices(ShopActions.Buying);
        Player.Instance().PlayerInventory.GetInventoryUI().UpdateInventory();
        Player.Instance().PlayerInventory.GetInventoryUI().SetShopUI(this);
        Player.Instance().PlayerInventory.GetInventoryUI().MakeInventoryClickable(true);
        RefreshShopAvailability();
    }

    public void CloseShop()
    {
        foreach (Transform obj in inventoryObjParent.transform)
            Destroy(obj.gameObject);
        Player.Instance().PlayerInventory.GetInventoryUI().MakeInventoryClickable(false);
    }

    // set items player can buy outlined and clickable
    public void RefreshShopAvailability()
    {
        foreach(InventoryItemUI itemUI in inventoryObjParent.GetComponentsInChildren<InventoryItemUI>())
        {
            itemUI.SetSellable(Player.Instance().CanAfford(itemUI.GetItemCost()));
        }
    }

    public override void SellItem(InventoryItemUI item)
    {
        // remove coins from player inventory
        Player.Instance().resources.AddCoins(-item.GetItemCost());
        PlayerInventoryUI playerUI = Player.Instance().PlayerInventory.GetInventoryUI();
        item.inventoryUI = playerUI;

        // add item to player inventory
        Player.Instance().PlayerInventory.AddItem(item.GetItem());
        // need to refresh clickable UI to set this new item
        playerUI.MakeInventoryClickable(true);
        // remove item from shop menu
        Remove(item);
        RefreshShopAvailability();
    }
}
