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
    public bool IsDiscovered = false;
    [SerializeField]
    public SecretLocale SecretDestination;
    [SerializeField]
    public string tooltip;

    public void Activate()
    {
        if (null == SecretDestination) {
            Debug.LogError("This secret doesn't point anywhere!", gameObject);
        }

        Debug.Log("New Secret Discovered!");
        SecretDestination.IsDiscovered = true;

        UIManager.Instance().OpenDialoguePopup("You learn a secret. You note its location on your map.");
        IsDiscovered = true;
    }
}

[RequireComponent(typeof(MapLocation))]
public abstract class SecretLocale : MonoBehaviour
    // Abstract class for places a secret can point to
{
    [SerializeField]
    public bool IsDiscovered = false;

    [SerializeField]
    public string Dialogue;

    public abstract void Activate();
}