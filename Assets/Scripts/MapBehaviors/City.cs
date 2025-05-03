using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : Locale
{
    public override string DefaultDescription => "You arrived at the city.";

    [SerializeField]
    private ShopItem[] shopItems;

    [SerializeField]
    GameObject insideShopPrefab;

    public override void SetupIconGraphics()
    {
        GetComponent<MapLocation>().GetIconGraphics().sprite = MapController.Instance().GetLocaleSprite(this);
    }
    private void Awake()
    {
        localeType = LocaleTypes.Shop;

        if (localeDescription == "")
            localeDescription = DefaultDescription;

        // create inside shop locale as child
        Instantiate(insideShopPrefab, transform);
    }
    public ShopItem[] GetShopItems() => shopItems;

    public void Talk()
    {

    }

    public void Sleep()
    {
        int cost = 5;
        BuyLodging(cost, LodgingDialogue(cost));
    }
    public string LodgingDialogue(int cost)
    {
        return "You spend " + cost + " coins to pay for an inn. The bed holds you, you fall asleep.";
    }
    
    public void BuyAndSell()
    {
        UIManager.Instance().OpenShopUI(this);
        UIManager.Instance().CloseLocalePopup();
        UIManager.Instance().OpenLocalePopup(GetComponentInChildren<InsideShop>());
    }
}
