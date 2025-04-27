using UnityEngine;

public enum Attitude {
    Friendly = 0,
    Neutral,
    Hostile
}

public class Cabin : Locale
{
    static string defaultShopLocaleDescription = "You arrived at the cabin.";

    public Attitude attitude;

    public override void Activate()
    {
        Debug.LogError("Locale not implemented: " + GetType());
    }
    public override void Deactivate()
    {
        Debug.LogError("Locale not implemented: " + GetType());
    }
    public override void SetupIconGraphics()
    {
        GetComponent<MapLocation>().GetIconGraphics().sprite = MapController.Instance().GetLocaleSprite(this);
    }


    private void Awake()
    {
        localeType = LocaleTypes.Shop;

        if (localeDescription == "")
            localeDescription = defaultShopLocaleDescription;
    }
    public void Talk()
    {

    }

    public void Sleep()
    {
        switch (attitude)
        {
            case Attitude.Neutral:
                int cost = 5;
                string neutral = "You spend " + cost + " coins to sleep here. The fireplace crackles, you fall asleep.";
                BuyLodging(5, neutral);
                return;
            case Attitude.Friendly:
                string friendly = "You are a welcomed guest and need not pay to sleep here. The fireplace crackles, you fall asleep.";
                FreeLodging(friendly);
                return;
            case Attitude.Hostile:
                string hostile = "The owner is hostile towards you and won't let you sleep";
                UIManager.Instance().OpenDialoguePopup(hostile);
                return;
        }
    }

}
