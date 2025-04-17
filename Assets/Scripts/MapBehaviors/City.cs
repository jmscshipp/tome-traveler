using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : Locale
{
    private string defaultShopLocaleDescription = "You arrived at the city.";

    [SerializeField]
    private ShopItem[] shopItems;

    [SerializeField]
    GameObject insideShopPrefab;
    public override void Activate()
    {
        Debug.LogError("Locale not implemented: " + this.GetType());
    }
    public override void Deactivate()
    {
        Debug.LogError("Locale not implemented: " + this.GetType());
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

    public void Talk()
    {

    }

    public void Sleep()
    {

    }

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
