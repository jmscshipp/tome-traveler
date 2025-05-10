using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : Locale
{
    
    public override string DefaultDescription => "You arrived at the town.";

    private void Awake()
    {
        localeType = LocaleTypes.Town;

        if (localeDescription == "")
            localeDescription = DefaultDescription;
    }

    public void Sleep()
    {
        Lodge();
    }

    public override string LodgingDialogue(int cost)
    {
        return "You spend " + cost + " coins to pay for an inn. The bed holds you, you fall asleep.";
    }

}
