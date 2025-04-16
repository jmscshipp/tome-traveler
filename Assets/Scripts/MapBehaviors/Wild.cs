using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wild : Locale
{
    RandomTable WildsRandomTable = new RandomTable(
        new List<RandomEvent>() {
            new GetRobbed(likelihood:1),
            new GetConcussed(likelihood: 1),
            new NothingHappens(likelihood: 20),
        });

    [SerializeField]
    public float HuntSuccessChance;

    public override void Activate()
    {
        Debug.Log("Activate Not Implemented");
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
