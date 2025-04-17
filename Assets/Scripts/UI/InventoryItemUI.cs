using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    private int itemCost;
    private Item item;
    private Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }
    public void Setup(Item assignedItem, Sprite graphic, int cost)
    {
        itemCost = cost;
        item = assignedItem;
        GetComponent<Image>().sprite = graphic;
    }

    public Item GetItem() => item;

    public void SetOutlineVisible(bool visible)
    {
        outline.enabled = visible;
    }
}
