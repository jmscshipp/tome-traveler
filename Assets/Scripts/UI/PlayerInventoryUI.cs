public class PlayerInventoryUI : InventoryUI
{
    public bool InShop = false;
    public ShopInventoryUI ShopUI;

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

    public void SetShopUI(InventoryUI ui) {
        foreach (InventoryItemUI uItem in inventoryObjParent.GetComponentsInChildren<InventoryItemUI>()) {
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
        MakeInventoryClickable(true);
        ShopUI.RefreshShopAvailability();
    }

    public void UpdateInventory()
    {
        //Debug.Log("UPDATING INVENTORY");
        Clear();
        foreach (Item i in Player.Instance().PlayerInventory.itemList)
        {
            Add(i);
        }

        if (InShop)
        {
            SetPrices(ShopActions.Selling);
            MakeInventoryClickable(true);
        }
    }
}
