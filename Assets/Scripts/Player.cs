
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    // variables for moving between locations
    [SerializeField]
    private AnimationCurve movementCurve;
    private bool traversing = false;
    public bool IsDead
    {
        get; private set;
    }
    
    private float traverseTimer = 0f;
    private float traverseSpeed = 10f;
    private Vector3 startPos;
    private Vector3 endPos;
    public MapLocation currentLocation;

    public PlayerResources resources;

    public Inventory PlayerInventory = new Inventory();

    public bool godmode = false;

    public bool StartWithTent = true;

    // Spell Classes
    public static Spell abundance = Spell.AllSpells[(int)Spells.Abundance];
    public static Spell waterwalking = Spell.AllSpells[(int)Spells.Waterwalking];
    public static Spell clairvoyance = Spell.AllSpells[(int)Spells.Clairvoyance];
    public static Spell mindreading = Spell.AllSpells[(int)Spells.Mindreading];
    public static Spell teleportation = Spell.AllSpells[(int)Spells.Teleportation];
    public static Spell sleepless = Spell.AllSpells[(int)Spells.Sleepless];
    public int StartingFood = 3;
    static Spell[] SpellLearnOrder;


    public void OnEnable()
    {
        if (SpellLearnOrder == null)
            SpellLearnOrder = Spell.AllSpells.OrderBy(x => Random.value).ToArray();
    }

    [SerializeField]
    public bool KnowsTeleportation ()
    {
        return godmode || teleportation.enabled;
    }
    public bool KnowsMindreading()
    {
        return godmode || mindreading.enabled;
    }
    public bool KnowsClairvoyance()
    {
        return godmode || clairvoyance.enabled;
    }
    public bool KnowsAbundance()
    {
        return godmode || abundance.enabled;
    }
    public bool KnowsWaterwalking()
    {
        return godmode || waterwalking.enabled;
    }
    public bool KnowsSleepless()
    {
        return godmode || sleepless.enabled;
    }

    [ContextMenu("Give random spell tome")]
    public void GiveTome()
    {
        PlayerInventory.AddItem(new Tome(RandomUnusedSpell()));
    }

    [ContextMenu("Give food")]
    public void GiveFood()
    {
        PlayerInventory.AddItem(new Food());
    }

    [ContextMenu("Give tent")]
    public void GiveTent()
    {
        PlayerInventory.AddItem(new Tent());
    }


    public List<Spell> GetSpells()
    {
        List<Spell> ss = new List<Spell>();
        foreach (Spell s in Spell.AllSpells)
        {
            if (s.enabled)
                ss.Add(s);
        }
        return ss;
    }

    public Spell LoseRandomSpell() {
        List<Spell> spells = GetSpells();
        int i = Random.Range(0, spells.Count);
        spells[i].enabled = false;
        return spells[i];
    }

    [SerializeField]
    public int InventorySizeLimit = 24;

    private static Player instance;

    public static Player Instance() => instance;

    private void Start()
    {
        currentLocation = MapController.Instance().startingLocation;
        IsDead = false;
        GetComponent<GameManager>().GameState.StarterSpell.enabled = true;
        PlayerInventory.tomeUI.Add(Item.ItemFromId(GetComponent<GameManager>().GameState.StarterSpell.ItemId));

        if (StartWithTent)
        {
            Tent t = new Tent();
            t.RandomizeUses();
            PlayerInventory.AddItem(t);
        }
        if (StartingFood > 0)
        {
            for (int i = 0; i< StartingFood; i++)
                PlayerInventory.AddItem(new Food());
        }

        resources = GetComponent<PlayerResources>();
        PlayerInventory.SizeLimit = InventorySizeLimit;
        // Clear prices from inventory display
        PlayerInventory.GetInventoryUI().SetPrices(ShopActions.None);
    }

    void Awake()
    {
        // setting up singleton
        if (instance != null && instance != this)
            Destroy(this);
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInventory.UpdateItemIndices();
        // Advance player position while traversing
        if (traversing)
        {
            traverseTimer += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, movementCurve.Evaluate(traverseTimer));
            if (traverseTimer > 1f)
                ArrivedAtLocation();
        }

        if (KnowsAbundance() && Input.GetKeyDown(KeyCode.A) && abundance.cooldown == 0)
        {
            abundance.Cast();
        }
        if (KnowsSleepless() && Input.GetKeyDown(KeyCode.S) && sleepless.cooldown == 0)
        {
            sleepless.Cast();
        }
        if (KnowsTeleportation() && Input.GetKeyDown(KeyCode.T) && teleportation.cooldown == 0)
        {
            teleportation.Cast();
        }
        if (KnowsMindreading() && Input.GetKeyDown(KeyCode.M) && mindreading.cooldown == 0)
        {
            mindreading.Cast();
        }
        if (KnowsClairvoyance() && Input.GetKeyDown(KeyCode.C) && clairvoyance.cooldown == 0)
        {
            clairvoyance.Cast();
        }
        if (KnowsWaterwalking() && Input.GetKeyDown(KeyCode.W) && waterwalking.cooldown == 0)
        {
            waterwalking.Cast();
        }
    }

    internal static Spell RandomUnusedSpell()
    {
        int i = 0;
        if (SpellLearnOrder == null)
            return Spell.AllSpells[0];
        while (SpellLearnOrder[i++].enabled) ;
        return SpellLearnOrder[i];
    }
    public bool CanAfford(int cost)
    {
        return resources.GetCoins() >= cost;
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

    public void EnableTomesForLearning()
    {
        PlayerInventory.ui.MakeTomesClickable(true);
    }

    public void Learn(Tome tome)
    {
        if (PlayerInventory.itemList.Contains(tome))
        {
            PlayerInventory.RemoveItem(tome);
        }
        PlayerInventory.tomeUI.Add(tome);
        tome.Spell.enabled = true;
    }

    public SleepWildernessResult SleepWilderness()
    {
        Tent tent = PlayerInventory.GetUsableTent();
        if (tent == null)
        {
            return new SleepWildernessResult(false);
        }
        bool tentBroke = tent.Use();
        resources.AddExhaustion(-GameManager.Instance().GameState.TentExhaustionReduction);
        PlayerInventory.ui.UpdateInventory();
        Spell.PassTime();

        SleepWildernessResult successResult = new SleepWildernessResult(true);
        if (tentBroke)
        {
            PlayerInventory.RemoveItem(tent);
            successResult.tentBroke = true;
            UIManager.Instance().OpenDialoguePopup("Your tent has broken in the night.");
        }
        return successResult;
    }

    public bool SleepBed()
    {
        resources.AddExhaustion(-GameManager.Instance().GameState.BedExhaustionReduction);
        Spell.PassTime();
        return true;
    }

    public void EatFood()
    {
        Food food = PlayerInventory.GetEdibleFood();
        if (food == null)
        {
            return;
        }

        bool foodEaten = food.Use();
        resources.AddHunger(-2);
        if (foodEaten)
        {
            PlayerInventory.RemoveItem(food);
        }
        
        UIManager.Instance().OpenDialoguePopup("You eat some food, filling your belly.");
        UIManager.Instance().OpenLocalePopup(currentLocation.GetLocale());
        UIManager.Instance().CloseLocalePopup();
        //return true;
    }


    public bool IsInWilderness()
    {
        return currentLocation.GetLocale() is Wild;
    }

    private void ArrivedAtLocation()
    {
        traversing = false;
        resources.AddExhaustion(1);
        resources.AddHunger(1);

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

        Spell.PassTime();
    }

    internal bool NextTo(MapLocation mapLocation)
    {
        return currentLocation.GetConnectedLocations().Contains(mapLocation);
    }

    internal static void Die()
    {
        Instance().IsDead = true;
        SceneManager.LoadScene(0);
        GameManager.WipeProgress();
    }
}
