using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmeraldCityHint : SecretLocale
{
    public override void Activate()
    {
        MapLocation.EmeraldCityHints += 1;
        if (MapLocation.EmeraldCityHints < MapLocation.EmeraldCityHintsNeeded)
        {

            UIManager.Instance().OpenDialoguePopup($"You learn a new Emerald City Hint. You now have {MapLocation.EmeraldCityHints}."); 
        } else
        {

            UIManager.Instance().OpenDialoguePopup($"You have all of the hints you need to find the city. But how will you get there?");
        }
    }
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }
}
