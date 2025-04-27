using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// base class with general inventory functionality for shops and player inventory to inherit from
public abstract class InventoryUI : MonoBehaviour
{
    [SerializeField]
    protected string itemDataDirectory = "Items"; // Assets/Resources/Cards
    [SerializeField]
    protected GameObject inventoryItemUIPrefab;
    [SerializeField]
    protected Transform inventoryObjParent;

    protected List<InventoryItemUI> items;
    public static List<ItemData> itemBlueprints = new List<ItemData>();

    // Start is called before the first frame update
    void Awake()
    {
        // load in item scriptable objects
        itemBlueprints.AddRange(Resources.LoadAll<ItemData>(itemDataDirectory));
        items = new List<InventoryItemUI>();
    }

    public static ItemData GetData(Items id) {
        return itemBlueprints.Find(x => x.ItemType == id);
    }

    public void SetPrices(ShopActions action)
    {
        foreach (InventoryItemUI ui in items)
        {
            SetPrice(action, ui);
        }
    }

    void SetPrice(ShopActions action, InventoryItemUI ui)
    {
        ItemData itemData = GetData(ui.GetItem().item_id);
        // set price based on if item is being bought from store or sold by player
        switch (action)
        {
            case ShopActions.Selling:
                ui.SetCost(itemData.BuyPrice);
                break;
            case ShopActions.Buying:
                ui.SetCost(itemData.SellPrice);
                break;
            case ShopActions.None:
                break;
        }
    }

    protected void Add(Item item)
    {
        GameObject newItemUI = Instantiate(inventoryItemUIPrefab, inventoryObjParent);
        InventoryItemUI itemui = newItemUI.GetComponent<InventoryItemUI>();
        itemui.inventoryUI = this;
        itemui.Setup(item, GetData(item.item_id).Sprite);
    }

    protected void Remove(Item item)
    {
        InventoryItemUI itemToRemove = items.Find(x => x.GetItem() == item);
        items.Remove(itemToRemove);
        Destroy(itemToRemove.gameObject);
    }

    public abstract void SellItem(InventoryItemUI itemUI);
}
