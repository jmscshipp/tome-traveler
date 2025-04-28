using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[SelectionBase]
public class MapLocation : MonoBehaviour
{
    [SerializeField]
    private List<MapLocation> connectedLocations = new List<MapLocation>();
    [SerializeField]
    bool connectionSetUp = false;
    private bool traversable = false;
    [SerializeField]
    private SpriteRenderer iconGraphics;
    [SerializeField]
    private GameObject highlightGraphics;
    [SerializeField]
    private bool finalLocation;
    private Material m_DefaultMaterial;
    [SerializeField]
    Material m_TeleportMaterial;

    public bool ActiveForTeleport { get; private set; }

    static internal List<MapLocation> activeTeleportLocations = new List<MapLocation>();


    private void Start()
    {
        iconGraphics.gameObject.SetActive(true);
        GetComponent<Locale>().SetupIconGraphics();
        if (null != highlightGraphics)
        {
            m_DefaultMaterial = highlightGraphics.GetComponent<Renderer>().material;
        }
        else
        {
            Debug.LogError("No highlight graphics set", this);
        }
    }

    public Locale GetLocale() {
        return GetComponent<Locale>();
    }

    private void OnMouseDown()
    {
        if (traversable)
            MapController.Instance().SendPlayerToNewLocation(this);

        if (ActiveForTeleport)
        {
            foreach (MapLocation m in activeTeleportLocations)
            {
                m.DeactivateForTeleport(isTraversable: connectedLocations.Contains(m));
                activeTeleportLocations.Remove(m);
            }
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
        traversable = isTraversable;
        highlightGraphics.SetActive(isTraversable);
    }

    [ContextMenu(nameof(ActivateForTeleport))]
    public void ActivateForTeleport()
    {
        ActiveForTeleport = true;
        SetTraversable(true, m_TeleportMaterial);
    }

    
    [ContextMenu(nameof(DeactivateForTeleport))]
    public void DeactivateForTeleport(bool isTraversable)
    {
        ActiveForTeleport = false;
        SetTraversable(isTraversable, m_DefaultMaterial);
    }

    private void SetTraversable(bool isTraversable, Material highlightMaterial)
    {
        traversable = isTraversable;
        highlightGraphics.SetActive(isTraversable);

        highlightGraphics.GetComponent<SpriteRenderer>().material = highlightMaterial;
    }

    public void ActivateLocation()
    {
        // the player has made it to the end of the game - the Emerald City
        if (finalLocation)
            UIManager.Instance().OpenDialoguePopup("You've made it! You've made it to the Emerald City!");
    }

    public void MakeConnectionsSelectable(bool selectable)
    {
        foreach (MapLocation location in connectedLocations)
            location.SetTraversable(selectable);
    }

    public void ActivateForTeleport(int repeats)
    // repeats should be a function of teleport spell strength
    {
        if (repeats > 0)
        {
            foreach (MapLocation location in connectedLocations)
            {
                // potential later optimization, not working at the moment
                if (true || !location.ActiveForTeleport)
                {
                    location.ActivateForTeleport();
                    location.ActivateForTeleport(repeats - 1);
                }
            }
        }
        else if (repeats == 0)
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
        else if (GetComponent<Wild>())
            sprite = "wilds.png";
        else if (GetComponent<Shop>())
            sprite = "shop.png";
        else if (GetComponent<City>())
            sprite = "city.png";
        else if (GetComponent<Ruins>())
            sprite = "ruins.png";
        else if (GetComponent<Cabin>())
            sprite = "cabin.png";

        Gizmos.DrawIcon(transform.position, "circle.png", true, Color.white);
        Gizmos.DrawIcon(transform.position, sprite, true, Color.black);
    }
}
