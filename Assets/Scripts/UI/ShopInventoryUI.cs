using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInventoryUI : MonoBehaviour
{
    [SerializeField]
    private string itemDataDirectory = "Items"; // Assets/Resources/Cards
    [SerializeField]
    private GameObject inventoryItemUIPrefab;
    [SerializeField]
    private Transform inventoryObjParent;

    List<InventoryItemUI> items;
    List<ItemData> itemBlueprints = new List<ItemData>();

    // Start is called before the first frame update
    void Awake()
    {
        // load in item scriptable objects
        itemBlueprints.AddRange(Resources.LoadAll<ItemData>(itemDataDirectory));
        items = new List<InventoryItemUI>();
    }

    // version for shops to use
    public void SetupShop(Shop shopLocation)
    {
        foreach(ShopItem shopItem in shopLocation.GetShopItems())
        {
            for (int i = 0; i < shopItem.quantity; i++)
                Add(new Item(shopItem.itemType));
        }
        RefreshShopAvailability();
    }

    // overriden version for cities to use
    public void SetupShop(City cityLocation)
    {
        foreach (ShopItem shopItem in cityLocation.GetShopItems())
        {
            for (int i = 0; i < shopItem.quantity; i++)
                Add(new Item(shopItem.itemType));
        }
        RefreshShopAvailability();
    }

    public void CloseShop()
    {
        foreach (Transform obj in inventoryObjParent.transform)
            Destroy(obj.gameObject);
    }

    // set items player can buy outlined and clickable
    public void RefreshShopAvailability()
    {
        foreach(InventoryItemUI itemUI in inventoryObjParent.GetComponentsInChildren<InventoryItemUI>())
        {
            // player doesn't have enough money for item
            if (itemUI.GetItem().cost > Player.Instance().GetPlayerResources().GetCoins())
                itemUI.SetOutlineVisible(false);
            // player has enough money for item
            else
                itemUI.SetOutlineVisible(true);
        }
    }

    public void Add(Item item)
    {
        Debug.Log("ADDING ITEM IN SHOP");

        GameObject newItemUI = Instantiate(inventoryItemUIPrefab, inventoryObjParent);
        ItemData itemData = itemBlueprints.Find(x => x.ItemType == item.item_id);
        newItemUI.GetComponent<InventoryItemUI>().Setup(item, itemData.Sprite, itemData.BuyPrice);
        items.Add(newItemUI.GetComponent<InventoryItemUI>());
    }

    public void SellItem(Item item)
    {
        InventoryItemUI itemToRemove = items.Find(x => x.GetItem() == item);
        items.Remove(itemToRemove);
        Destroy(itemToRemove.gameObject);
    }
}
