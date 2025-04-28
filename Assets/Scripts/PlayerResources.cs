using System;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    [Header("Number of hunger / exhaustion at which player dies")]
    [SerializeField]
    private int maxHunger = 5;
    [SerializeField]
    private int maxExhaustion = 5;
    [SerializeField]
    private int coins = 0;

    [SerializeField]
    public PlayerResourcesUI ui;

    private int hunger = 0;
    private int exhaustion = 0;

    private void Start()
    {
        // start the game with no hunger or exhaustion
        AddHunger(-10);
        AddExhaustion(-10);

        // start the game with 10 coins
        AddCoins(10);
    }

    public int GetMaxHunger() => maxHunger;
    public int GetMaxExhaustion() => maxExhaustion;

    public int GetHunger() => hunger;

    public void AddHunger(int modifier)
    {
        //Debug.Log("Changing player hunger by " + modifier);
        hunger = Math.Clamp(hunger + modifier, 0, 5);
        ui.UpdateHungerUI(hunger, maxHunger);
        if (hunger == 5)
            UIManager.Instance().OpenDialoguePopup("You died of hunger!");
            // here is where we'll tie in the actual player death event
    }
    public int GetExhaustion() => exhaustion;
    public void AddExhaustion(int modifier)
    {
        exhaustion = Math.Clamp(exhaustion + modifier, 0, 5);
        ui.UpdateExhaustionUI(exhaustion, maxExhaustion);
        if (hunger == 5)
            UIManager.Instance().OpenDialoguePopup("You died of exhaustion!");
            // here is where we'll tie in the actual player death event
    }

    public int GetCoins() => coins;
    public void AddCoins(int modifier)
    {
        //Debug.Log("adding coins");
        coins = Math.Clamp(coins + modifier, 0, 1000);
        ui.UpdateCoinsUI(coins);
    }
}
