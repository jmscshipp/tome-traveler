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
    [SerializeField]
    public bool KnowsTeleportation = false;
    [SerializeField]
    public bool KnowsMindreading = false;
    [SerializeField]
    public bool KnowsSleepless = false;
    [SerializeField]
    public bool KnowsAbundance = false;
    [SerializeField]
    public bool KnowsWaterwalking = false;
    [SerializeField]
    public bool KnowsClairvoyance = false;

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

    public bool Learn(Tome tome)
    {
        switch (tome.item_id)
        {
            case Items.AbundanceTome:
                KnowsAbundance = true;
                break;
            case Items.ClairvoyanceTome:
                KnowsClairvoyance = true;
                break;
            case Items.MindreadingTome:
                KnowsMindreading = true;
                break;
            case Items.SleeplessTome:
                KnowsSleepless = true;
                break;
            case Items.TeleportationTome:
                KnowsTeleportation = true;
                break;
            case Items.WaterwalkingTome:
                KnowsWaterwalking = true;
                break;
            default:
                Debug.LogError("Error: Can't Learn Tome because no case in switch statement for " + tome.item_id);
                return false;
        }
        return true;
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

        // After we have arrived, check to find out what kind of location we're in
        // We do this by checking the current location, which has just been updated
        Secret secret = currentLocation.GetComponent(typeof(Secret)) as Secret;
        if (secret != null)
        {
            // reveal the secret to the player, update UI, etc
            secret.Activate();
        }

        Locale locale = currentLocation.GetComponent(typeof(Locale)) as Locale;
        if (locale != null)
        {
            // open the locale UI window
            // disable any actions that aren't permitted during UI interactions
            // disable and enable any shop-specific actions
            locale.Activate();
            
        }

        Wild wild = currentLocation.GetComponent(typeof(Wild)) as Wild;
        if (wild != null)
        {
            // open the shop UI window
            // disable any actions that aren't permitted during UI interactions
            // disable and enable any shop-specific actions
        }
    }
}
