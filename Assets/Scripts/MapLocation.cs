using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;

[SelectionBase]
public class MapLocation : MonoBehaviour
{
    public static int EmeraldCityHints = 0;
    public readonly static int EmeraldCityHintsNeeded = 4;
    [SerializeField]
    private List<MapLocation> connectedLocations = new List<MapLocation>();
    [SerializeField]
    bool connectionSetUp = false;
    private bool Hidable = true;
    public bool Traversable { get; private set; }
    [SerializeField]
    private SpriteRenderer iconGraphics;
    [SerializeField]
    private GameObject highlightGraphics;
    [SerializeField]
    private bool finalLocation;

    [SerializeField]
    Material m_DefaultMaterial;
    [SerializeField]
    Material m_TeleportMaterial;
    [SerializeField]
    Material m_MindreadingMaterial;
    [SerializeField]
    Material m_SecretLocaleMaterial;


    public bool ActiveForTeleport { get; private set; }

    static internal List<MapLocation> activeTeleportLocations = new List<MapLocation>();
    static internal List<MapLocation> activeMindreadingLocations = new List<MapLocation>();


    private void Start()
    {
        CheckNullConnections();
        iconGraphics.gameObject.SetActive(true);
        GetComponent<Locale>().SetupIconGraphics();
        if (null == highlightGraphics)
        {
            Debug.LogError("No highlight graphics set", this);
        }
    }

    public void CheckNullConnections()
    {
        foreach (MapLocation l in connectedLocations)
        {
            if (l == null)
            {
                Debug.LogWarning("This maplocation has a null connection!", this);
            }
        }
        while (connectedLocations.Contains(null))
        {
            connectedLocations.Remove(null);
        }
    }

    public Locale GetLocale()
    {
        return GetComponent<Locale>();
    }

    private void OnMouseDown()
    {
        if (Traversable)
            MapController.Instance().SendPlayerToNewLocation(this);

        if (ActiveForTeleport)
        {
            Teleportation.Cleanup();
        }
    }

    public List<MapLocation> GetConnectedLocations() => connectedLocations;
    public bool GetConnectionSetUp() => connectionSetUp;
    public SpriteRenderer GetIconGraphics() => iconGraphics;

    public void ConnectTwoWays(MapLocation other)
    {
        if (!connectedLocations.Contains(other))
        {
            connectedLocations.Add(other);
        }
        if (!other.GetConnectedLocations().Contains(this))
        {
            other.GetConnectedLocations().Add(this);
        }
    }
    
    public void SetConnectionSetup(bool setUp)
    {
        connectionSetUp = setUp;
    }

    public void SetTraversable(bool isTraversable)
    {
        Traversable = isTraversable;
        SetHighlight(Traversable);
    }

    [ContextMenu(nameof(ActivateForTeleport))]
    internal void ActivateForTeleport()
    {
        ActiveForTeleport = true;
        activeTeleportLocations.Add(this);
        SetTraversable(true);
        SetHighlightMaterial(m_TeleportMaterial);
    }

    [ContextMenu(nameof(ActivateForMindreading))]
    internal void ActivateForMindreading()
    {
        activeMindreadingLocations.Add(this);
        // For now mindreading detects secrets AND secret locales.
        // TODO: Decide if we want to change this
        if (null != GetComponent<SecretLocale>() || null != GetComponent<Secret>())
            SetHighlightMaterial(m_MindreadingMaterial);
    }

    internal void DeactivateForMindreading()
    {
        SetHighlightMaterial(m_DefaultMaterial);
    }

    [ContextMenu(nameof(DeactivateForTeleport))]
    public void DeactivateForTeleport(bool isTraversable)
    {
        ActiveForTeleport = false;
        SetTraversable(isTraversable);
        SetHighlightMaterial(m_DefaultMaterial);
    }

    public void SetHighlight(bool active)
    {
        if (!active && Hidable)
            highlightGraphics.SetActive(false);
        if (active)
            highlightGraphics.SetActive(true);
    }

    private void SetHighlightMaterial(Material highlightMaterial)
    {
        highlightGraphics.GetComponent<SpriteRenderer>().material = highlightMaterial;
    }

    public void ActivateLocation()
    {
        // the player has made it to the end of the game - the Emerald City
        if (finalLocation && EmeraldCityHints >= EmeraldCityHintsNeeded)
            UIManager.Instance().OpenDialoguePopup("You've made it! You've made it to the Emerald City!");
    }
    public void ActivateSecretLocale()
    {
        m_DefaultMaterial = m_SecretLocaleMaterial;
        SetHighlightMaterial(m_SecretLocaleMaterial);
        Hidable = false;
        SetHighlight(true);
    }

    public void MakeConnectionsSelectable(bool selectable)
    {
        foreach (MapLocation location in connectedLocations)
            location.SetTraversable(selectable);
    }
    public void ActivateForMindreading(int SpellStrength)
    // repeats are a function of spell strength
    {
        if (SpellStrength > 0)
        {
            foreach (MapLocation location in connectedLocations)
            {
                if (location == Player.Instance().currentLocation)
                    continue;

                location.ActivateForMindreading();
                location.ActivateForMindreading(SpellStrength - 1);
            }
        }
        else if (SpellStrength == 0)
        {
            foreach (MapLocation location in connectedLocations)
                location.ActivateForMindreading();
        }
        else
        {
            Debug.LogError("ActivateForMindreading repeats cannot be negative", this);
        }
    }

    public void ActivateForTeleport(int SpellStrength)
    // repeats are a function of spell strength
    {
        if (SpellStrength > 0)
        {
            foreach (MapLocation location in connectedLocations)
            {
                if (location == Player.Instance().currentLocation)
                    continue;
                // potential later optimization, not working at the moment
                if (true || !location.ActiveForTeleport)
                {
                    location.ActivateForTeleport();
                    location.ActivateForTeleport(SpellStrength - 1);
                }
            }
        }
        else if (SpellStrength == 0)
        {
            foreach (MapLocation location in connectedLocations)
                location.ActivateForTeleport();
        }
        else
        {
            Debug.LogError("ActivateForTeleport repeats cannot be negative", this);
        }
    }

    private void OnDrawGizmos()
    {
        string sprite = "";
        if (GetComponent<Town>())
            sprite = "town.png";
        else if (GetComponent<Ruins>())
            sprite = "ruins.png";
        else if (GetComponent<Wild>())
            sprite = "wilds.png";
        else if (GetComponent<Shop>())
            sprite = "shop.png";
        else if (GetComponent<City>())
            sprite = "city.png";
        else if (GetComponent<Cabin>())
            sprite = "cabin.png";

        Gizmos.DrawIcon(transform.position, "circle.png", true, Color.white);
        Gizmos.DrawIcon(transform.position, sprite, true, Color.black);
    }
}
