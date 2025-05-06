using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureCache : SecretLocale
{

    public override void Activate()
    {
        new FindItem(likelihood: 0, new Treasure()).Activate();
        UIManager.Instance().OpenDialoguePopup("You follow the secret instructions to the letter, and find a treasure chest half-buried in the ground. Eureka!");
    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }
}
