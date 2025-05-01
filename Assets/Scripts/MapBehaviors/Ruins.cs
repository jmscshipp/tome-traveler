using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruins : Wild
{
    private string defaultDescription = "You arrived at the ruins.";

    public override void SetupIconGraphics()
    {
        GetComponent<MapLocation>().GetIconGraphics().sprite = MapController.Instance().GetLocaleSprite(this);
    }

    private void Awake()
    {
        localeType = LocaleTypes.Shop;

        if (localeDescription == "")
            localeDescription = defaultDescription;
    }

    //Exploring a ruin is harder but more rewarding
    // Getting a tome is pretty unlikely
    // We can make a few different random tables and choose between them
    // including one with a guaranteed or preferred spell tome
    RandomTable RuinsRandomTable = new RandomTable(
        new List<RandomEvent>() {
            new GetRobbed(likelihood:1),
            new GetConcussed(likelihood: 1),
            new FindItem(likelihood: 4, new Tome(Player.RandomUnusedSpell())),
            new FindItem(likelihood: 4, new Treasure()),
            new FindItem(likelihood: 8, new Tent()),
            new FindItem(likelihood: 8, new Food()),
            new NothingHappens(likelihood: 20),
        });

    public override void Explore()
    {
        RuinsRandomTable.ChooseRandom().Activate();
    }
}
