using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameState", order = 1)]
public class GameState : ScriptableObject
{
    public Spell StarterSpell;
    public int ExploreExhaustionPenalty;
    public int MaxFoodFromHunt;
    public int MaxRestFromTent;
}