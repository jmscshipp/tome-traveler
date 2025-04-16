using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wild : Locale
{
    [SerializeField]
    public float HuntSuccessChance;

    public override void Activate()
    {
        Debug.Log("Activate Not Implemented");
    }

    public void Hunt(Player player)
    {
        if (Random.Range(0, 1) < HuntSuccessChance)
        {
            if (player.PlayerInventory.HasSpace())
            {
                player.PlayerInventory.AddItem(new Food());
            }
        }
    }

    public void Camp(Player player)
    {
        player.SleepWilderness();
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
