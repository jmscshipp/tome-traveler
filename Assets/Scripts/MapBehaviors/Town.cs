using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : Locale
{
    private string defaultTownLocaleDescription = "You arrived at the town.";

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
        localeType = LocaleTypes.Town;

        if (localeDescription == "")
            localeDescription = defaultTownLocaleDescription;
    }

    public void Sleep()
    {

    }

    public void Talk()
    {

    }

    public void Explore()
    {

    }
}
