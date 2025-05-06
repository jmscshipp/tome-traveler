using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    bool sellable;
    [SerializeField]
    private Item item;
    [SerializeField]
    private Image graphics;
    private Outline outline;
    public InventoryUI inventoryUI;

    [SerializeField]
    public Items id = Items.None;

    // This is a debug field
    [SerializeField]
    int InventoryIndex = -1;
    private bool learnable = false;


    private void Awake()
    {
        outline = GetComponentInChildren<Outline>();
    }
    public void Setup(Item assignedItem, Sprite graphic)
    {
        item = assignedItem;
        graphics.sprite = graphic;
        id = assignedItem.item_id;
        InventoryIndex = item.GetIndex();
    }

    public void SetCost(int cost)
    {
        item.cost = cost;
        GetComponentInChildren<TextMeshProUGUI>().SetText(cost.ToString());
    }
    public void ClearCost()
    {
        GetComponentInChildren<TextMeshProUGUI>().SetText("");
    }

    public int GetItemCost()
    {
        return item.cost;
    }

    public Item GetItem() => item;

    // the shop tells this ui element if the player has enough money to buy it
    // if they don't, turn off highlights and make it unclickable!
    // if in player inventory, item should always be sellable
    public void SetSellable(bool isSellable)
    {
        sellable = isSellable;
        outline.enabled = sellable;
    }

    public void SetLearnable(bool value)
    {
        learnable = value;
        outline.enabled = value;
    }

    // when mouse is over this UI element, 
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("hovering over inventory UI item index " + item.GetIndex());
        if (!sellable && !learnable)
            return;
        outline.effectColor = Color.white;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (!sellable && !learnable)
            return;
        outline.effectColor = Color.black;
    }

    // when this UI element is clicked
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (!sellable && !learnable)
            return;

        if (sellable)
            inventoryUI.SellItem(this);

        if (learnable)
            inventoryUI.LearnItem(this);
    }
}