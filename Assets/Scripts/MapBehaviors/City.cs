using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : Locale
{
    private string defaultShopLocaleDescription = "You arrived at the city.";

    public override void Activate()
    {
        Debug.LogError("Locale not implemented: " + this.GetType());
    }
    public override void Deactivate()
    {
        Debug.LogError("Locale not implemented: " + this.GetType());
    }


    private void Awake()
    {
        localeType = LocaleTypes.Shop;

        if (localeDescription == "")
            localeDescription = defaultShopLocaleDescription;
    }

    public void Talk()
    {

    }

    public void Sleep()
    {

    }

    public void Buy()
    {

    }

    public void Sell()
    {

    }

    public void Explore()
    {

    }
}
