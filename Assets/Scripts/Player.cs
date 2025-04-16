using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{
    // variables for moving between locations
    [SerializeField]
    private AnimationCurve movementCurve;
    private bool traversing = false;
    private float traverseTimer = 0f;
    private float traverseSpeed = 10f;
    private Vector2 startPos;
    private Vector2 endPos;
    private MapLocation currentLocation;

    private PlayerResources playerResources;

    public Inventory PlayerInventory = new Inventory();

    private void Awake()
    {
        playerResources = GetComponent<PlayerResources>();
    }

    // Update is called once per frame
    void Update()
    {
        // Advance player position while traversing
        if (traversing)
        {
            traverseTimer += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, movementCurve.Evaluate(traverseTimer));
            if (traverseTimer > 1f)
                ArrivedAtLocation();
        }
    }

    public void TraverseToNewLocation(MapLocation newLocation)
    {
        // don't go to new location if already on the way to one
        if (traversing)
            return;

        startPos = transform.position;
        endPos = newLocation.transform.position;
        currentLocation = newLocation;
        traverseTimer = 0f;
        traversing = true;
    }

    public bool Sleep()
    {
        // check if in town
        // check if in wilderness
        //if in wilderness, see if u can sleep
        Tent tent = PlayerInventory.GetUsableTent();
        if (tent == null)
        {
            // We failed to find a usable tent so return false
            return false;
        }

        // This is where you'd update the exhaustion meter
        return true;
    }

    private void ArrivedAtLocation()
    {
        traversing = false;
        playerResources.AddExhaustion(1);
        playerResources.AddHunger(1);
        currentLocation.ActivateLocation();
    }
}
