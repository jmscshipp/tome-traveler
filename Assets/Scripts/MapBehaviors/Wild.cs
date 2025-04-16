using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wild : Locale
{
    private string defaultWildLocaleDescription = "You arrive at the wildnerness.";

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
        localeType = LocaleTypes.Wilds;

        if (localeDescription == "")
            localeDescription = defaultWildLocaleDescription;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
