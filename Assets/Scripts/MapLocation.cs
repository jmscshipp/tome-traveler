using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLocation : MonoBehaviour
{
    [SerializeField]
    private MapLocation[] connectedLocations;
    private bool connectionSetUp = false;
    private bool traversable = false;
    [SerializeField]
    private GameObject highlightGraphics;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (traversable)
            MapController.Instance().SendPlayerToNewLocation(this);
    }

    public MapLocation[] GetConnectedLocations() => connectedLocations;
    public bool GetConnectionSetUp() => connectionSetUp;
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
        foreach (MapLocation location in connectedLocations)
            location.SetTraversable(true);
    }
}
