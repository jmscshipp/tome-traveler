using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : Locale
{
    
    public override string DefaultDescription => "You arrived at the town.";

    public override void SetupIconGraphics()
    {
        GetComponent<MapLocation>().GetIconGraphics().sprite = MapController.Instance().GetLocaleSprite(this);
    }
    private void Awake()
    {
        localeType = LocaleTypes.Town;

        if (localeDescription == "")
            localeDescription = DefaultDescription;
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

    public void Talk()
    {

    }
}
