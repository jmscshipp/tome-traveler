using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruins : Wild
{
    public override string DefaultDescription => "You arrived at the ruins.";

    private void Awake()
    {
        localeType = LocaleTypes.Ruins;

        if (localeDescription == "")
            localeDescription = DefaultDescription;
    }

    //Exploring a ruin is harder but more rewarding
    // Getting a tome is pretty unlikely
    // We can make a few different random tables and choose between them
    // including one with a guaranteed or preferred spell tome
    RandomTable RuinsRandomTable = new RandomTable(
        new List<RandomEvent>() {
            new GetRobbed(likelihood:1),
            // new GetConcussed(likelihood: 1),
            new FindItem(likelihood: 4, new Treasure()),
            new FindItem(likelihood: 8, new Tent()),
            new FindItem(likelihood: 8, new Food()),
            new NothingHappens(likelihood: 10, ShowDialogue: true),
        });

    public override void Explore()
    {
        if (Searched)
        {
            UIManager.Instance().OpenDialoguePopup("You've already searched these ruins.");
            return;
        }

        SecretLocale secretLocale = GetComponent(typeof(SecretLocale)) as SecretLocale;
        if (secretLocale != null)
        {
            UIManager.Instance().OpenDialoguePopup("After exploring all day, you find something peculiar...!");
            secretLocale.Activate();
        }
        System.Action penalty = () => Player.Instance().GetComponent<PlayerResources>().AddExhaustion(gm.GameState.ExploreExhaustionPenalty);
        UIManager.Instance().QueueActionAfterPopup(penalty);
        RuinsRandomTable.ChooseRandom().Activate();
        Searched = true;
    }
}
