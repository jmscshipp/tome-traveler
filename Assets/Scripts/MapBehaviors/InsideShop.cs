using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideShop : Locale
{
    private string defaultShopLocaleDescription = "You peruse the shopkeeper's wares, and try to pawn off your own findings.";

    public override void SetupIconGraphics()
    {
        // doesn't use this function
    }

    private void Awake()
    {
        localeType = LocaleTypes.InsideShop;

        if (localeDescription == "")
            localeDescription = defaultShopLocaleDescription;
    }

    public void LeaveShop()
    {
        UIManager.Instance().CloseLocalePopup();
        UIManager.Instance().CloseShopUI();

        // finds parent city or shop and reopens it
        Locale parentLocale = transform.parent.GetComponent(typeof(Locale)) as Locale;
        UIManager.Instance().OpenLocalePopup(parentLocale);
    }
}
