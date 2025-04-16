using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : Locale
{
    private string defaultShopLocaleDescription = "You arrived at the shop.";
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
        localeType = LocaleTypes.Shop;

        if (localeDescription == "")
            localeDescription = defaultShopLocaleDescription;
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
