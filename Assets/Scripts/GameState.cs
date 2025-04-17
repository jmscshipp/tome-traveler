using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameState", order = 1)]
public class GameState : ScriptableObject
{
    public Spell StarterSpell;
    public int ExploreExhaustionPenalty;
    public int MaxFoodFromHunt;
    public int MaxRestFromTent;
    public float HuntExhaustionChance;
    public int HuntExhaustionPenalty;

    public void SetStarterSpellAbundance()
    {
        StarterSpell = new Abundance();
    }
    public void SetStarterSpellWaterwalking()
    {
        StarterSpell = new Waterwalking();
    }
    public void SetStarterSpellSleepless()
    {
        StarterSpell = new Sleepless();
    }
    public void SetStarterSpellClairvoyance()
    {
        StarterSpell = new Clairvoyance();
    }
    public void SetStarterSpellMindreading()
    {
        StarterSpell = new Mindreading();
    }

    public void LoadEmeraldCity()
    {
        SceneManager.LoadScene(1);
    }
}