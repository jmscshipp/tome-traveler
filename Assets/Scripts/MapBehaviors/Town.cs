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

    private void Awake()
    {
        localeType = LocaleTypes.Town;

        if (localeDescription == "")
            localeDescription = defaultTownLocaleDescription;
    }

    // Start is called before the first frame update
    void Start()
    {
        //localeDescription
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
