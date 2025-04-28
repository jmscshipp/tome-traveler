using System.Collections.Generic;
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

    public static List<ItemData> itemBlueprints = new List<ItemData>();

    // Start is called before the first frame update
    void Awake()
    {
        // load in item scriptable objects
        itemBlueprints.AddRange(Resources.LoadAll<ItemData>(itemDataDirectory));
    }

    public static ItemData GetData(Items id) {
        return itemBlueprints.Find(x => x.ItemType == id);
    }

    public void SetPrices(ShopActions action)
    {
        foreach (InventoryItemUI ui in inventoryObjParent.GetComponentsInChildren<InventoryItemUI>())
        {
            SetPrice(action, ui);
        }
    }

    public void Clear()
    {
        foreach (InventoryItemUI i in inventoryObjParent.GetComponentsInChildren<InventoryItemUI>())
        {
            Remove(i);
        }
    }

    void SetPrice(ShopActions action, InventoryItemUI ui)
    {
        ItemData itemData = GetData(ui.GetItem().item_id);
        // set price based on if item is being bought from store or sold by player
        switch (action)
        {
            case ShopActions.Selling:
                ui.SetCost(itemData.SellPrice);
                break;
            case ShopActions.Buying:
                ui.SetCost(itemData.BuyPrice);
                break;
            case ShopActions.None:
                ui.ClearCost();
                break;
        }
    }

    protected InventoryItemUI Add(Item item)
    {
        GameObject newItemUI = Instantiate(inventoryItemUIPrefab, inventoryObjParent);
        InventoryItemUI itemui = newItemUI.GetComponent<InventoryItemUI>();
        itemui.inventoryUI = this;
        itemui.Setup(item, GetData(item.item_id).Sprite);
        return itemui;
    }

    protected void Remove(int inventoryIndex)
    {
        foreach (InventoryItemUI uitem in inventoryObjParent.GetComponentsInChildren<InventoryItemUI>())
        {
            if (uitem.GetItem().GetIndex() == inventoryIndex)
            {
                Destroy(uitem.gameObject);
            }
        }
    }
    protected void Remove(Item item)
    {
        foreach (InventoryItemUI uitem in inventoryObjParent.GetComponentsInChildren<InventoryItemUI>())
        {
            if (uitem.GetItem() == item)
            {
                Destroy(uitem.gameObject);
                return;
            }
        }

        Debug.LogError("Could not remove item; doesn't exist. " + item.item_id, gameObject);
    }

    protected void Remove(InventoryItemUI uitem)
    {
        Destroy(uitem.gameObject);
    }

    public abstract void SellItem(InventoryItemUI itemUI);
}
