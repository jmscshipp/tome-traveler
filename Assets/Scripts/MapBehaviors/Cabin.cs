using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabin : Locale
{
    private string defaultShopLocaleDescription = "You arrived at the cabin.";

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

    public void Explore()
    {

    }
}
