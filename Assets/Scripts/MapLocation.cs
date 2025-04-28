using System.Collections.Generic;
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

    private void Start()
    {
        iconGraphics.gameObject.SetActive(true);
        GetComponent<Locale>().SetupIconGraphics();
    }

    public Locale GetLocale() {
        return GetComponent<Locale>();
    }

    private void OnMouseDown()
    {
        if (traversable)
            MapController.Instance().SendPlayerToNewLocation(this);
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
