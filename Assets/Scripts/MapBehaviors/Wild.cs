using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wild : Locale
{
    private string defaultWildLocaleDescription = "You arrive at the wildnerness.";

    RandomTable WildsRandomTable = new RandomTable(
        new List<RandomEvent>() {
            new GetRobbed(likelihood:1),
            new GetConcussed(likelihood: 1),
            new NothingHappens(likelihood: 20),
        });

    [SerializeField]
    public float HuntSuccessChance = .35f;

    public override void Activate()
    {
        Debug.Log("Activate Not Implemented");
    }

    private void Awake()
    {
        localeType = LocaleTypes.Wilds;

        if (localeDescription == "")
            localeDescription = defaultWildLocaleDescription;
    }

    public void Hunt()
    {
        if (Random.Range(0, 1) < HuntSuccessChance)
        {
            if (player.PlayerInventory.HasSpace())
            {
                int num_food = Random.Range(1, gm.GameState.MaxFoodFromHunt);
                for (int i =0; i<num_food; i++)
                    player.PlayerInventory.AddItem(new Food());

                UIManager.Instance().OpenDialoguePopup("You hunt and find " + num_food + " food.");
                UIManager.Instance().OpenLocalePopup(this);
                UIManager.Instance().CloseLocalePopup();
            }
        } else
        {
            UIManager.Instance().OpenDialoguePopup("You hunt but find no food.");
            UIManager.Instance().OpenLocalePopup(this);
            UIManager.Instance().CloseLocalePopup();
        }

        WildsRandomTable.ChooseRandom().Activate();
    }

    public void Camp()
    {
        bool result = player.SleepWilderness();
        if (result)
        {
            int exhaustionReduction = Random.Range(1, gm.GameState.MaxRestFromTent);
            Player.Instance().GetComponent<PlayerResources>().AddExhaustion(-gm.GameState.ExploreExhaustionPenalty);
            UIManager.Instance().OpenDialoguePopup("You curl up in your sleeping bag and watch the stars. In the morning, you feel refreshed.");
            UIManager.Instance().OpenLocalePopup(this);
            UIManager.Instance().CloseLocalePopup();
        }
        else
        {
            UIManager.Instance().OpenDialoguePopup("You have nothing suitable to sleep with here. It isn't safe.");
            UIManager.Instance().OpenLocalePopup(this);
            UIManager.Instance().CloseLocalePopup();
        }
        // Trigger a random event
        WildsRandomTable.ChooseRandom().Activate();
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
            UIManager.Instance().OpenLocalePopup(this);
            UIManager.Instance().CloseLocalePopup();
        } else
        {
            UIManager.Instance().OpenDialoguePopup("You search to the point of exhaustion, but there is nothing to find.");
            UIManager.Instance().OpenLocalePopup(this);
            UIManager.Instance().CloseLocalePopup();
        }
    }

    public override void Deactivate()
    {
        throw new System.NotImplementedException();
    }
}
