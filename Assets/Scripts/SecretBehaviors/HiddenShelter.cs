using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenShelter : SecretLocale
{
    public override void Activate()
    {
        GetComponent<Locale>().ActivateHiddenShelter();
        UIManager.Instance().OpenDialoguePopup("You discover a hidden shelter in this location. You can always sleep here for free.");
    }
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
