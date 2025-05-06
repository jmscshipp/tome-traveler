using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopItem
{
    public Items itemType;
    public int quantity;

    public ShopItem(Items itemType, int quantity)
    {
        this.itemType = itemType;
        this.quantity = quantity;
    }
}

public class Shop : Locale
{
    private string defaultShopLocaleDescription = "You arrived at the shop.";

    [SerializeField]
    private List<ShopItem> shopItems;
    [SerializeField]
    GameObject insideShopPrefab;

    // setting up representationn of locale on the map
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

    public void Start()
    {
        if (shopItems.Count == 0)
        {
            RestockShop();
        }
    }

    public void RestockShop()
    {
        shopItems.Clear();
        float p_Food = .1f;
        float p_Tent = .1f;
        int numFood = 0;
        int numTent = 0;
        int num_tries = 12;
        for (int i = 0; i < num_tries; i++)
        {
            if (Random.Range(0f,1f) < p_Food)
            {
                numFood += 1;
            }
            if (Random.Range(0f,1f) < p_Tent)
            {
                numTent += 1;
            }
        }
        shopItems.Add(new ShopItem(Items.Food, numFood));
        shopItems.Add(new ShopItem(Items.Tent, numTent));
    }

    public IEnumerable<ShopItem> GetShopItems() => shopItems;

    public void BuyAndSell()
    {
        UIManager.Instance().OpenShopUI(this);
        UIManager.Instance().CloseLocalePopup();
        UIManager.Instance().OpenLocalePopup(GetComponentInChildren<InsideShop>());
    }
}
