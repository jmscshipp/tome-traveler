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
    internal static List<SecretLocale> UndiscoveredSecretLocales = new List<SecretLocale>();
    internal static List<SecretLocale> DiscoveredSecretLocales = new List<SecretLocale>();
    public bool IsDiscovered = false;
    [SerializeField]
    public SecretLocale SecretDestination;
    [SerializeField]
    public string tooltip;

    internal static void ActivateRandomSecretLocale()
    {
        int i = Random.Range(0, UndiscoveredSecretLocales.Count);
        SecretLocale l = UndiscoveredSecretLocales[i];
        UndiscoveredSecretLocales.RemoveAt(i);
        l.IsDiscovered = true;
        l.Activate();
    }


    public void Activate()
    {
        if (null == SecretDestination)
        {
            Debug.LogError("This secret doesn't point anywhere!", gameObject);
        }

        Debug.Log("New Secret Discovered!", SecretDestination);
        SecretDestination.IsDiscovered = true;
        UndiscoveredSecretLocales.Remove(SecretDestination);

        UIManager.Instance().OpenDialoguePopup("You learn a secret. You note its location on your map.");
        IsDiscovered = true;

        SecretDestination.GetComponent<MapLocation>().ActivateSecretLocale();
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

    public void Start()
    {
        Secret.UndiscoveredSecretLocales.Add(this);
    }

    public abstract void Activate();
}