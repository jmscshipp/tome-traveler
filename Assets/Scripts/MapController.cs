using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MapController : MonoBehaviour
{
    [SerializeField]
    private MapLocation startingLocation;
    private MapLocation[] allLocations;
    [SerializeField]
    private GameObject connectionPrefab;
    [SerializeField]
    private Transform connectionParent;
    [SerializeField]
    private Player player;

    private MapLocation currentLocation;
    private static MapController instance;

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
        allLocations = new MapLocation[locationObjects.Length];
        for (int i = 0; i < locationObjects.Length; i++)
            allLocations[i] = locationObjects[i].GetComponent<MapLocation>();

        ConnectionSetup(startingLocation);

        // position player at start
        player.transform.position = startingLocation.transform.position;
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
        Gizmos.color = Color.white;
        foreach(GameObject location in GameObject.FindGameObjectsWithTag("MapLocation"))
        {
            foreach (MapLocation connectedLocation in location.GetComponent<MapLocation>().GetConnectedLocations())
                Gizmos.DrawLine(location.transform.position, connectedLocation.transform.position);
        }
    }
    
    public void SendPlayerToNewLocation(MapLocation location)
    {
        ResetLocationsTraversability();
        player.TraverseToNewLocation(location);
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
}
