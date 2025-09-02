using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameState", order = 1)]
public class GameState : ScriptableObject
{
    public Spell StarterSpell = Player.abundance;
    public int ExploreExhaustionPenalty;
    public int MaxFoodFromHunt;
    public int MinFoodFromHunt = 1;
    public int MinRestFromTent = 1;
    public int MaxRestFromTent;
    public float HuntExhaustionChance;
    public int HuntExhaustionPenalty;
    public int TentExhaustionReduction = 2;
    public int BedExhaustionReduction = 3;

    public void SetStarterSpellAbundance()
    {
        StarterSpell = Player.abundance;
    }
    public void SetStarterSpellWaterwalking()
    {
        StarterSpell = Player.waterwalking;
    }
    public void SetStarterSpellSleepless()
    {
        StarterSpell = Player.sleepless;
    }
    public void SetStarterSpellClairvoyance()
    {
        StarterSpell = Player.clairvoyance;
    }
    public void SetStarterSpellMindreading()
    {
        StarterSpell =  Player.mindreading;
    }

    public void LoadEmeraldCity()
    {
        SceneManager.LoadScene(1);
    }
}