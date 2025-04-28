using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum LocaleTypes : ushort
{
    None = 0,
    Town,
    Wilds,
    Shop,
    InsideShop, // this exists for UI purposes!
    City,
    Cabin,
    Ruins
}

[RequireComponent(typeof(MapLocation))]
public abstract class Locale : MonoBehaviour
{
    protected Player player;
    protected GameManager gm;
    private void Start()
    {
        player = Player.Instance();
        gm = GameManager.Instance();
    }

    [SerializeField]
    protected string localeDescription;
    protected LocaleTypes localeType;

    public abstract void Activate();

    public abstract void Deactivate();

    public abstract void SetupIconGraphics();

    public string GetLocaleDescription() => localeDescription;
    public LocaleTypes GetLocaleType() => localeType;

    // this function only exists as a sneaky workaround to get inside the shop working, otherwise don't use!!
    public void SetLocalType(LocaleTypes type)
    {
        localeType = type;
    }

    public void Explore()
    {
        // if no secret, give nothing
        // if secret, give it 
        SecretLocale secretLocale = GetComponent(typeof(SecretLocale)) as SecretLocale;
        //add exhaustion as penalty
        Player.Instance().GetComponent<PlayerResources>().AddExhaustion(gm.GameState.ExploreExhaustionPenalty);

        if (secretLocale != null)
        {
            UIManager.Instance().OpenDialoguePopup("After exploring all day, you find something peculiar...!");
            secretLocale.Activate();
        }
        else
        {
            UIManager.Instance().OpenDialoguePopup("You search to the point of exhaustion, but there is nothing to find.");
        }
    }

    public void BuyLodging(int cost, string dialogue)
    {
        if (Player.Instance().resources.GetCoins() < cost)
        {
            return;
        }
        Player.Instance().SleepBed();
        Player.Instance().resources.AddCoins(-cost);
        UIManager.Instance().OpenDialoguePopup(dialogue);
    }
    public void FreeLodging(string dialogue)
    {
        Player.Instance().SleepBed();
        UIManager.Instance().OpenDialoguePopup(dialogue);
    }
}