using System;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField]
    public MapLocation startingLocation;
    private MapLocation[] allLocations;
    [SerializeField]
    private GameObject connectionPrefab;
    [SerializeField]
    private Transform connectionParent;
    [SerializeField]
    private Player player;

    private MapLocation currentLocation;
    private static MapController instance;

    // icons for different locales
    [SerializeField]
    private Sprite townMapIcon;
    [SerializeField]
    private Sprite wildsMapIcon;
    [SerializeField]
    private Sprite shopMapIcon;
    [SerializeField]
    private Sprite cityMapIcon;
    [SerializeField]
    private Sprite cabinMapIcon;
    [SerializeField]
    private Sprite ruinsMapIcon;

    [SerializeField]
    public float SecretGizmoDrawSize = 3f;


    public static MapController Instance() => instance;
    private void Awake()
    {
        // setting up singleton
        if (instance != null && instance != this)
            Destroy(this);
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // set up list of all locations on the map
        GameObject[] locationObjects = GameObject.FindGameObjectsWithTag("MapLocation");
        List<MapLocation> locs = new List<MapLocation>();
        foreach (GameObject g in locationObjects)
        {
            MapLocation m = g.GetComponent<MapLocation>();
            if (null == m)
                continue;
            locs.Add(m);
            m.CheckNullConnections();
        }
        allLocations = locs.ToArray();

        ConnectionSetup(startingLocation);

        // position player at start
        player.transform.position = startingLocation.transform.position;
        currentLocation = startingLocation;
        startingLocation.ActivateLocation();
        startingLocation.MakeConnectionsSelectable(true);
    }

    private void CreateConnectionGraphic(MapLocation location, MapLocation connectedLocation)
    {
        // create connection graphic
        Vector3 dir = location.transform.position - connectedLocation.transform.position;
        Vector2 connectionLocation = (location.transform.position + connectedLocation.transform.position) / 2f;
        GameObject connection = Instantiate(connectionPrefab, connectionLocation, Quaternion.identity, connectionParent);
        connection.transform.localScale = new Vector3(0.25f, dir.magnitude, 1f);
        connection.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
    }

    public void RevealSecretConnection(MapLocation location, MapLocation secretConnection)
    {
        if (secretConnection == null)
        {
            Debug.LogError("Null secret connection!");
            return;
        }
        CreateConnectionGraphic(location, secretConnection);
    }

    // recursively explore map to set up connection graphics
    private void ConnectionSetup(MapLocation location)
    {
        location.SetConnectionSetup(true);
        foreach (MapLocation connectedLocation in location.GetConnectedLocations())
        {
            CreateConnectionGraphic(location, connectedLocation);

            // only set up connections for this location if it's unexplored
            if (connectedLocation.GetConnectionSetUp() != true)
                ConnectionSetup(connectedLocation);
        }
    }

    // debug lines to be able to see connections in editor
    void OnDrawGizmos()
    {
        foreach(GameObject location in GameObject.FindGameObjectsWithTag("MapLocation"))
        {
            MapLocation l = location.GetComponent<MapLocation>();
            if (null == l) continue;
            // show shortcut connections as a blue line
            if (l.GetComponent<HiddenShortcut>() is HiddenShortcut hs && hs != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(location.transform.position, hs.SecretDestination.transform.position);
            }
            if (l.GetComponent<SecretLocale>() is SecretLocale sl && sl != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(location.transform.position, SecretGizmoDrawSize);
            }
            if (l.GetComponent<Secret>() is Secret st && st != null)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(location.transform.position, SecretGizmoDrawSize);
                Gizmos.color = Color.magenta;
                if (st.SecretDestination)
                    Gizmos.DrawLine(location.transform.position, st.SecretDestination.transform.position);
            }
            foreach (MapLocation connectedLocation in location.GetComponent<MapLocation>().GetConnectedLocations())
            {
                Gizmos.color = Color.white;
                if (connectedLocation == null) continue;
                if (connectedLocation.GetConnectedLocations().Contains(l))
                {
                    Gizmos.DrawLine(location.transform.position, connectedLocation.transform.position);
                }
                else
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(location.transform.position, connectedLocation.transform.position);

                }
            }
        }
    }

    public void SendPlayerToNewLocation(MapLocation location)
    {
        ResetLocationsTraversability();
        player.TraverseToNewLocation(location);
        Mindreading.Cleanup();
        Teleportation.Cleanup();
    }

    // disables highlight and makes locations unclickable while player travels to current location
    public void ResetLocationsTraversability()
    {
        foreach(MapLocation location in allLocations)
            location.SetTraversable(false);
    }

    public MapLocation GetCurrentLocation() => currentLocation;

    public void SetCurrentLocation(MapLocation location)
    {
        currentLocation = location;
    }

    public Sprite GetLocaleSprite(Locale locale)
    {
        Sprite sprite = null;
        switch (locale.GetLocaleType())
        {
            case LocaleTypes.Town:
                sprite = townMapIcon;
                break;
            case LocaleTypes.Wilds:
                sprite = wildsMapIcon;
                break;
            case LocaleTypes.Shop:
                sprite = shopMapIcon;
                break;
            case LocaleTypes.City:
                sprite = cityMapIcon;
                break;
            case LocaleTypes.Ruins:
                sprite = ruinsMapIcon;
                break;
            case LocaleTypes.Cabin:
                sprite = cabinMapIcon;
                break;
        }
        return sprite;
    }
}
