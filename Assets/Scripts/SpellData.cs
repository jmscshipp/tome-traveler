using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpellData", order = 1)]
public class SpellData : ScriptableObject
{
    [SerializeField]
    public string Name;
    public int Cooldown = 3;
    public int Strength = 1;
}
