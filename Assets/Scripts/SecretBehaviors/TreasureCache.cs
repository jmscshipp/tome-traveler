using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureCache : SecretLocale
{

    public override void Activate()
    {
        Player.Instance().PlayerInventory.AddItem(new Treasure());
        UIManager.Instance().OpenDialoguePopup("You find a treasure chest half-buried in the ground. Eureka!");
    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }
}
