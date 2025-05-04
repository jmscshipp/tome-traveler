

public class Cabin : Locale
{
    static string defaultShopLocaleDescription = "You arrived at the cabin.";

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

    public void Sleep()
    {
        Lodge();
    }
    
    public override string LodgingDialogue(int cost)
    {
        Player.Instance().resources.AddHunger(-1);
        return "You spend " + cost + " coins to sleep in this cabin. The fireplace crackles, you fall asleep comfortably.";
    }
    public override string FriendlyLodgingDialogue()
    {
        Player.Instance().resources.AddHunger(-2);
        Player.Instance().resources.AddExhaustion(-1);
        return "You are a welcomed guest and talk with your host over dinner. The fireplace crackles, you fall asleep.";
    }
    public override string HostileLodgingDialogue()
    {
        return "The owner is hostile and asks you to leave their home.";
    }

}
