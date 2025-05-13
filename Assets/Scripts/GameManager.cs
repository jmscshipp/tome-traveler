using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;

    public static GameManager Instance() => instance;

    private void Awake()
    {
        // setting up singleton
        if (instance != null && instance != this)
            Destroy(this);
        instance = this;

        // load in item scriptable objects
        itemBlueprints.AddRange(Resources.LoadAll<ItemData>(itemDataDirectory));
    }

    [SerializeField]
    public GameState GameState;
    [SerializeField]
    protected string itemDataDirectory = "Items"; // Assets/Resources/Cards
    
    static float fogRadius = 5f;

    public float debugFowRadius = 5f;
    
    public static List<ItemData> itemBlueprints = new List<ItemData>();

    public static ItemData GetData(Items id)
    {
        return itemBlueprints.Find(x => x.ItemType == id);
    }

    internal static void WipeProgress()
    {
        // for now do nothing
        // when we have serialization, this will delete unsaved progress
        // re-randomize the world, etc
    }

    public static event Action FoWUpdate;

    public static float GetFogOfWarRadius()
    {
        return fogRadius;
    }

    [ContextMenu("Update Fog of War")]
    public void DebugUpdateFogOfWar()
    {
        UpdateFogOfWarRadius(debugFowRadius);
    }
    
    public static void UpdateFogOfWarRadius(float newRadius)
    {
        fogRadius = newRadius;
        FoWUpdate?.Invoke();
    }
}
