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
                player.PlayerInventory.AddItem(new Food());
            }
        }

        WildsRandomTable.ChooseRandom().Activate();
    }

    public void Camp()
    {
        player.SleepWilderness();
        // Trigger a random event
        WildsRandomTable.ChooseRandom().Activate();
    }

    public void Explore()
    {

    }

    public override void Deactivate()
    {
        throw new System.NotImplementedException();
    }
}
