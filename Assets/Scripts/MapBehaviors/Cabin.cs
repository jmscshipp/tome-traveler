

public class Cabin : Locale
{
    static string defaultShopLocaleDescription = "You arrived at the cabin.";

    private void Awake()
    {
        localeType = LocaleTypes.Cabin;

        if (localeDescription == "")
            localeDescription = defaultShopLocaleDescription;
    }

    public void Sleep()
    {
        Lodge();
    }
    
    public override string LodgingDialogue(int cost)
    {
        Player.Instance().resources.AddHunger(-2);
        Player.Instance().resources.AddExhaustion(-2);
        return "You spend " + cost + " coins to sleep in this cabin. You enjoy a hot meal. The fireplace crackles, you fall asleep comfortably.";
    }
    public override string FriendlyLodgingDialogue()
    {
        Player.Instance().resources.AddHunger(-3);
        Player.Instance().resources.AddExhaustion(-3);
        return "You are a welcomed guest and talk with your host over dinner. You eat together and talk into the night. The fireplace crackles, you fall asleep.";
    }
    public override string HostileLodgingDialogue()
    {
        return "The owner is hostile and asks you to leave their home.";
    }

}
