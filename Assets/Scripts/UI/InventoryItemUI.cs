using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    private Item item;

    public void Setup(Item assignedItem, Sprite graphic)
    {
        item = assignedItem;
        GetComponent<Image>().sprite = graphic;
    }

    public Item GetItem() => item;
}
