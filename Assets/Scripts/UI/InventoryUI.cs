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
    private List<ItemData> itemBlueprints = new List<ItemData>();

    // Start is called before the first frame update
    void Awake()
    {
        // load in item scriptable objects
        itemBlueprints.AddRange(Resources.LoadAll<ItemData>(itemDataDirectory));
        items = new List<InventoryItemUI>();
    }

    public void Add(Item item, ShopActions action)
    {
        GameObject newItemUI = Instantiate(inventoryItemUIPrefab, inventoryObjParent);
        ItemData itemData = itemBlueprints.Find(x => x.ItemType == item.item_id);
        // set price based on if item is being bought from store or sold by player
        if (action == ShopActions.Buying)
            newItemUI.GetComponent<InventoryItemUI>().Setup(item, itemData.Sprite, itemData.BuyPrice);
        else
            newItemUI.GetComponent<InventoryItemUI>().Setup(item, itemData.Sprite, itemData.SellPrice);

        AssignInventoryUI(newItemUI.GetComponent<InventoryItemUI>());
        items.Add(newItemUI.GetComponent<InventoryItemUI>());
    }

    public void Remove(Item item)
    {
        InventoryItemUI itemToRemove = items.Find(x => x.GetItem() == item);
        items.Remove(itemToRemove);
        Destroy(itemToRemove.gameObject);
    }

    public abstract void SellItem(InventoryItemUI itemUI);
    public abstract void AssignInventoryUI(InventoryItemUI itemUI);
}
