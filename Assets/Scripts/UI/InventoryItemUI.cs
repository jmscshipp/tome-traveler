using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private bool sellable;
    private int itemCost = 0;
    private Item item;
    [SerializeField]
    private Image graphics;
    private Outline outline;
    public InventoryUI inventoryUI;

    private void Awake()
    {
        outline = GetComponentInChildren<Outline>();
    }
    public void Setup(Item assignedItem, Sprite graphic)
    {
        item = assignedItem;
        graphics.sprite = graphic;
        GetComponentInChildren<TMP_Text>().text = itemCost.ToString();
    }

    public void SetCost(int cost)
    {
        this.itemCost = cost;
    }

    public int GetItemCost() => itemCost;
    public Item GetItem() => item;

    // the shop tells this ui element if the player has enough money to buy it
    // if they don't, turn off highlights and make it unclickable!
    // if in player inventory, item should always be sellable
    public void SetSellable(bool isSellable)
    {
        sellable = isSellable;
        outline.enabled = sellable;
    }

    // when mouse is over this UI element, 
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("hovering over inventory UI item");
        if (!sellable)
            return;
        outline.effectColor = Color.white;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (!sellable)
            return;
        outline.effectColor = Color.black;
    }

    // when this UI element is clicked
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (!sellable)
            return;
        inventoryUI.SellItem(this);
    }
}