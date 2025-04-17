using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruins : Locale
{
    private string defaultShopLocaleDescription = "You arrived at the ruins.";


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

    public void Explore()
    {

    }
}
