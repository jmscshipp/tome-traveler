using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SecretType
{
    FoodCache,
    TreasureCache,
    HiddenShelter,
    EmeraldCityHint,
    HiddenShortcut,
    TomeLocation,
}

[RequireComponent(typeof(MapLocation))]
public class Secret : MonoBehaviour
{
    [SerializeField]
    public SecretLocale SecretDestination;
    [SerializeField]
    public string tooltip;

    public void Activate()
    {
        Debug.Log("New Secret Discovered!");
        SecretDestination.IsDiscovered = true;
    }
}

[RequireComponent(typeof(MapLocation))]
public abstract class SecretLocale : MonoBehaviour
    // Abstract class for places a secret can point to
{
    [SerializeField]
    internal bool IsDiscovered;

    public abstract void Activate();
}