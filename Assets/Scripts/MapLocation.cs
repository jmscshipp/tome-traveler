using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLocation : MonoBehaviour
{
    [SerializeField]
    private List<MapLocation> connectedLocations = new List<MapLocation>();
    private bool connectionSetUp = false;
    private bool traversable = false;
    [SerializeField]
    private GameObject highlightGraphics;
    [SerializeField]
    private bool finalLocation;

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

    public List<MapLocation> GetConnectedLocations() => connectedLocations;
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
        // the player has made it to the end of the game - the Emerald City
        if (finalLocation)
            UIManager.Instance().OpenDialoguePopup("You've made it! You've made it to the Emerald City!");
    }

    public void MakeConnectionsSelectable(bool selectable)
    {
        foreach (MapLocation location in connectedLocations)
            location.SetTraversable(selectable);
    }
}
