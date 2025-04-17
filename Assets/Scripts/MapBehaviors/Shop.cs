using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopItem
{
    public Items itemType;
    public int quantity;

}

public class Shop : Locale
{
    private string defaultShopLocaleDescription = "You arrived at the shop.";

    [SerializeField]
    private ShopItem[] shopItems;
    [SerializeField]
    GameObject insideShopPrefab;

    public override void Activate()
    {
        Debug.LogError("Unimplemented function!!");
    }

    public override void Deactivate()
    {
        throw new System.NotImplementedException();
    }

    public override void SetupIconGraphics()
    {
        GetComponent<MapLocation>().GetIconGraphics().sprite = MapController.Instance().GetLocaleSprite(this);
    }

    private void Awake()
    {
        localeType = LocaleTypes.Shop;

        if (localeDescription == "")
            localeDescription = defaultShopLocaleDescription;

        // create inside shop locale as child
        Instantiate(insideShopPrefab, transform);
    }

    public ShopItem[] GetShopItems() => shopItems;

    public void BuyAndSell()
    {
        UIManager.Instance().OpenShopUI(this);
        UIManager.Instance().CloseLocalePopup();
        UIManager.Instance().OpenLocalePopup(GetComponentInChildren<InsideShop>());
    }

    public void Explore()
    {

    }
}
