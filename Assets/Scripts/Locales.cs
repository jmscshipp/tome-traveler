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

public enum Attitude {
    Friendly = 0,
    Neutral,
    Hostile
}

[RequireComponent(typeof(MapLocation))]
public abstract class Locale : MonoBehaviour
{
    protected bool IsHiddenShelterDiscovered = false;
    protected Player player;
    protected GameManager gm;
    private void Start()
    {
        player = Player.Instance();
        gm = GameManager.Instance();
    }

    public virtual string DefaultDescription
    {
        get => "Default Locale description";
    }

    [SerializeField]
    protected string localeDescription;
    protected LocaleTypes localeType;

    public abstract void SetupIconGraphics();

    public string GetLocaleDescription() => localeDescription;
    public LocaleTypes GetLocaleType() => localeType;

    // this function only exists as a sneaky workaround to get inside the shop working, otherwise don't use!!
    public void SetLocalType(LocaleTypes type)
    {
        localeType = type;
    }

    public virtual void Explore()
    {
        // if no secret, give nothing
        // if secret, give it 
        SecretLocale secretLocale = GetComponent(typeof(SecretLocale)) as SecretLocale;
        //add exhaustion as penalty
        Player.Instance().resources.AddExhaustion(gm.GameState.ExploreExhaustionPenalty);

        if (secretLocale != null && !secretLocale.IsDiscovered)
        {
            UIManager.Instance().OpenDialoguePopup("After exploring all day, you find something peculiar...!");
            secretLocale.Activate();
        }
        else
        {
            UIManager.Instance().OpenDialoguePopup("You search to the point of exhaustion, but there is nothing to find.");
        }
    }

    public void Lodge()
    {
        if (CheckHiddenShelter()) return;
        switch (NPCAttitude)
        {
            case Attitude.Neutral:
                int cost = LodgingCost();
                BuyLodging(cost, LodgingDialogue(cost));
                return;
            case Attitude.Friendly:
                FreeLodging(FriendlyLodgingDialogue());
                return;
            case Attitude.Hostile:
                UIManager.Instance().OpenDialoguePopup(HostileLodgingDialogue());
                return;
        }
    }

    public bool CheckHiddenShelter()
    {
        if (IsHiddenShelterDiscovered)
        {
            FreeLodging("You retreat to your shelter, alone but quite safe.");
            return true;
        }
        return false;
    }

    public virtual int LodgingCost()
    {
        return 5;
    }
    public virtual string LodgingDialogue(int cost)
    {
        return "You spend " + cost + " coins to sleep here.";
    }
    public virtual string FriendlyLodgingDialogue()
    {
        return "You are a welcomed guest and need not pay to sleep here. The fireplace crackles, you fall asleep.";
    }
    public virtual string HostileLodgingDialogue()
    {
        return "The owner is hostile towards you and won't let you sleep";
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

    RandomTable NPCRandomTable = new RandomTable(
        new List<RandomEvent>()
        {
            new GiveRandomSecret(likelihood:3),
            new GiveSupplies(likelihood:5),
            new NothingHappens(likelihood:10),
        }
    );
    public Attitude NPCAttitude = Attitude.Neutral;

    public void Talk()
    {
        // Auto-succeed if mindreading has charges
        Mindreading m = (Mindreading)Player.mindreading;
        bool mindreadingActive = m.HasCharges();
        if (mindreadingActive)
        {
            // trigger mindreading dialogue box
            TalkSuccess();
            return;
        }

        if (NPCAttitude == Attitude.Hostile)
        {
            TalkFailure();
            return;
        }

        if (GetComponent<SecretLocale>() is SecretLocale s && s != null && s.IsDiscovered)
        {
            // trigger secret dialogue box
            TalkSuccess();
            return;
        }

        // if none of the above, it's random
        int roll = Random.Range(0, 20);
        if (roll >= 15)
        {
            TalkSuccess();
            return;
        }
        else
        {
            TalkFailure();
            return;
        }
    }

    protected virtual void TalkSuccess()
    {
        UIManager.Instance().OpenDialoguePopup("You and the NPC get to talking, and really hit it off. They offer to let you stay for free.");
        NPCAttitude = Attitude.Friendly;
        NPCRandomTable.ChooseRandom().Activate();
    }
    protected virtual void TalkFailure()
    {
        UIManager.Instance().OpenDialoguePopup("You do something to make the NPC frustrated. They won't let you stay here anymore, even if you pay.");
        NPCAttitude = Attitude.Hostile;
    }

    internal void ActivateHiddenShelter()
    {
        throw new System.NotImplementedException();
    }
}