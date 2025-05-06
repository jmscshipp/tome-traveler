using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyCache : SecretLocale
{
    public override void Activate()
    {
        // this is a hack. likelihood isn't used here
        new GiveSupplies(likelihood:0).Activate();
    }
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }
}
