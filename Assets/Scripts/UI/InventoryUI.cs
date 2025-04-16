using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryUI : MonoBehaviour
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
    void Start()
    {
        // load in item scriptable objects
        itemBlueprints.AddRange(Resources.LoadAll<ItemData>(itemDataDirectory));
        items = new List<InventoryItemUI>();

        // set up reference to inventory
        GameObject.FindWithTag("Player").GetComponent<Player>().PlayerInventory.SetupUIRefs(this);
    }

    public void Add(Item item)
    {
        GameObject newItemUI = Instantiate(inventoryItemUIPrefab, inventoryObjParent);
        newItemUI.GetComponent<InventoryItemUI>().Setup(item, itemBlueprints.Find(x => x.ItemType == item.item_id).Sprite);
        items.Add(newItemUI.GetComponent<InventoryItemUI>());
    }

    public void Remove(Item item)
    {
        InventoryItemUI itemToRemove = items.Find(x => x.GetItem() == item);
        items.Remove(itemToRemove);
        Destroy(itemToRemove.gameObject);
    }
}
