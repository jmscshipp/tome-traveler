using System.Collections;
using System.Collections.Generic;
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
    private PlayerResourcesUI playerResourcesUI;

    private int hunger = 0;
    private int exhaustion = 0;

    private void Start()
    {
        // start the game with no hunger or exhaustion
        AddHunger(-10);
        AddExhaustion(-10);

        // start the game with 10 coins
        AddCoins(55);
    }

    public int GetMaxHunger() => maxHunger;
    public int GetMaxExhaustion() => maxExhaustion;

    public int GetHunger() => hunger;

    public void AddHunger(int modifier)
    {
        hunger = (int)Mathf.Clamp(hunger + modifier, 0f, 5f);
        playerResourcesUI.UpdateHungerUI(hunger, maxHunger);
        if (hunger == 5)
            UIManager.Instance().OpenDialoguePopup("You died of hunger!");
            // here is where we'll tie in the actual player death event
    }
    public int GetExhaustion() => exhaustion;
    public void AddExhaustion(int modifier)
    {
        exhaustion = (int)Mathf.Clamp(exhaustion + modifier, 0f, 5f);
        playerResourcesUI.UpdateExhaustionUI(exhaustion, maxExhaustion);
        if (hunger == 5)
            UIManager.Instance().OpenDialoguePopup("You died of exhaustion!");
            // here is where we'll tie in the actual player death event
    }

    public int GetCoins() => coins;
    public void AddCoins(int modifier)
    {
        Debug.Log("adding coins");
        coins = (int)Mathf.Clamp(coins + modifier, 0f, 1000f);
        playerResourcesUI.UpdateCoinsUI(coins);
    }
}
