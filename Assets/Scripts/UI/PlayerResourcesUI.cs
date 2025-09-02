using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerResourcesUI : MonoBehaviour
{
    // parent gameobjects that pips are under
    [SerializeField]
    Transform hungerPips;
    [SerializeField]
    Transform exhaustionpips;

    // graphics used to display pips
    [SerializeField]
    private Sprite emptyPipGraphic;
    [SerializeField]
    private Sprite fullPipGraphic;

    [SerializeField]
    TextMeshProUGUI coinsText;

    [SerializeField]
    TextMeshProUGUI daysText;

    public void UpdateHungerUI(int hunger, int maxHunger)
    {
        Image[] pips = hungerPips.GetComponentsInChildren<Image>();
        for (int i = 0; i < maxHunger; i++)
        {
            if (i <= hunger)
                pips[i].sprite = fullPipGraphic;
            else
                pips[i].sprite = emptyPipGraphic;
        }
    }

    public void UpdateExhaustionUI(int exhaustion, int maxExhaustion)
    {
        Image[] pips = exhaustionpips.GetComponentsInChildren<Image>();
        for (int i = 0; i < maxExhaustion; i++)
        {
            if (i <= exhaustion)
            {
                //Debug.Log("full exhaustion pip");
                pips[i].sprite = fullPipGraphic;
            }
            else
            {
                //Debug.Log("empty exhaustion pip");
            
                pips[i].sprite = emptyPipGraphic;
            }
        }
    }

    public void UpdateCoinsUI(int coins)
    {
        coinsText.SetText("Coins: {0}", coins);
    }

    public void UpdateDaysUI(int days)
    {
        daysText.SetText("Days: {0}", days);
    }
}
