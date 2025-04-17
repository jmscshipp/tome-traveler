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

    public PlayerResources GetPlayerResources() => playerResources;

    [SerializeField]
    public bool KnowsTeleportation ()
    {
        return Spells.Exists(x => x is Teleportation);
    }
    public bool KnowsMindreading()
    {
        return Spells.Exists(x => x is Mindreading);
    }
    public bool KnowsClairvoyance()
    {
        return Spells.Exists(x => x is Clairvoyance);
    }
    public bool KnowsAbundance()
    {
        return Spells.Exists(x => x is Abundance);
    }
    public bool KnowsWaterwalking()
    {
        return Spells.Exists(x => x is Waterwalking);
    }
    public bool KnowsSleepless()
    {
        return Spells.Exists(x => x is Sleepless);
    }

    public List<Spell> Spells = new List<Spell>();

    [SerializeField]
    public int InventorySizeLimit = 24;


    private static Player instance;

    public static Player Instance() => instance;

    private void Awake()
    {
        // setting up singleton
        if (instance != null && instance != this)
            Destroy(this);
        instance = this;

        playerResources = GetComponent<PlayerResources>();
        PlayerInventory.SizeLimit = InventorySizeLimit;
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
        MapController.Instance().SetCurrentLocation(currentLocation);
        traverseTimer = 0f;
        traversing = true;
    }

    public void Learn(Tome tome)
    {
        Spells.Add(tome.Spell);
    }

    public bool SleepWilderness()
    {
        Tent tent = PlayerInventory.GetUsableTent();
        if (tent == null)
        {
            // We failed to find a usable tent so return false
            return false;
        }

        // This is where you'd update the exhaustion meter
        return true;
    }

    public bool IsInWilderness()
    {
        return currentLocation.GetComponent(typeof(Wild)) as Wild != null;
    }

    private void ArrivedAtLocation()
    {
        traversing = false;
        playerResources.AddExhaustion(1);
        playerResources.AddHunger(1);

        // After we have arrived, check to see if there are any secrets in our new location
        // We do this by checking the current location, which has just been updated
        Secret secret = currentLocation.GetComponent(typeof(Secret)) as Secret;
        if (secret != null && !secret.IsDiscovered)
        {
            // reveal the secret to the player, update UI, etc
            secret.Activate();
        }

        // After checking for secrets, check to see if we're inside of a secret that we've already discovered
        // If we haven't found a secret revealing this Locale yet, we don't get the secret locale!
        SecretLocale secretLocale = currentLocation.GetComponent(typeof(SecretLocale)) as SecretLocale;
        if (secretLocale != null && secretLocale.IsDiscovered)
        {
            // reveal the secret to the player, update UI, etc
            secretLocale.Activate();
        }

        currentLocation.ActivateLocation();

        // After we have arrived, check to find out what kind of location we're in
        // We do this by checking the current location, which has just been updated
        // The normal Locale UI popup should happen *after* the SecretLocale UI popup
        Locale locale = currentLocation.GetComponent(typeof(Locale)) as Locale;
        if (locale != null)
        {
            // open the locale UI window
            // disable any actions that aren't permitted during UI interactions
            // disable and enable any shop-specific actions
            UIManager.Instance().OpenLocalePopup(locale);
        }
    }
}
