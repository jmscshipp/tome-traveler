using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wild : Locale
{
    public override string DefaultDescription => "You arrive at the wildnerness.";

    RandomTable WildsRandomTable = new RandomTable(
        new List<RandomEvent>() {
            new GetRobbed(likelihood:1),
            new GetConcussed(likelihood: 1),
            new NothingHappens(likelihood: 20),
        });

    public float HuntSuccessChance = .35f;

    public override void SetupIconGraphics()
    {
        GetComponent<MapLocation>().GetIconGraphics().sprite = MapController.Instance().GetLocaleSprite(this);
    }

    private void Awake()
    {
        localeType = LocaleTypes.Wilds;

        if (localeDescription == "")
            localeDescription = DefaultDescription;
    }

    public void Hunt()
    {
        if (Random.Range(0f, 1f) < gm.GameState.HuntExhaustionChance)
        {
            Player.Instance().GetComponent<PlayerResources>().AddExhaustion(gm.GameState.HuntExhaustionPenalty);
        }

        if (Random.Range(0f, 1f) < HuntSuccessChance)
        {
            int num_food = Random.Range(1, gm.GameState.MaxFoodFromHunt);
            for (int i =0; i<num_food; i++)
            {
                if (player.PlayerInventory.HasSpace())
                    player.PlayerInventory.AddItem(new Food());
            }
            UIManager.Instance().OpenDialoguePopup("You hunt and find " + num_food + " food.");
        } else
        {
            UIManager.Instance().OpenDialoguePopup("You hunt but find no food.");
        }

        WildsRandomTable.ChooseRandom().Activate();
    }

    public void Camp()
    {
        SleepWildernessResult result = player.SleepWilderness();
        if (result.succeeded)
        {
            int exhaustionReduction = Random.Range(1, gm.GameState.MaxRestFromTent);
            Player.Instance().GetComponent<PlayerResources>().AddExhaustion(-exhaustionReduction);
            UIManager.Instance().OpenDialoguePopup("You curl up in your sleeping bag and watch the stars. In the morning, you feel refreshed.");
            if (result.tentBroke)
                UIManager.Instance().OpenDialoguePopup("Your tent has broken in the night.");
        }
        else
        {
            UIManager.Instance().OpenDialoguePopup("You have nothing suitable to sleep with here. It isn't safe.");
        }
        // Trigger a random event
        WildsRandomTable.ChooseRandom().Activate();
    }

}

public class SleepWildernessResult
{
    public bool succeeded;
    public bool tentBroke = false;

    public SleepWildernessResult(bool succeeded)
    {
        this.succeeded = succeeded;
    }

}