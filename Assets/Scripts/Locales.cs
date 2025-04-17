using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum LocaleTypes : ushort
{
    None = 0,
    Town,
    Wilds,
    Shop,
    InsideShop, // this exists for UI purposes!
    City,
    Cabin,
    Ruins
}

[RequireComponent(typeof(MapLocation))]
public abstract class Locale : MonoBehaviour
{
    protected Player player;
    protected GameManager gm;
    private void Start()
    {
        player = Player.Instance();
        gm = GameManager.Instance();
    }

    [SerializeField]
    protected string localeDescription;
    protected LocaleTypes localeType;

    public abstract void Activate();

    public abstract void Deactivate();

    public string GetLocaleDescription() => localeDescription;
    public LocaleTypes GetLocaleType() => localeType;

    // this function only exists as a sneaky workaround to get inside the shop working, otherwise don't use!!
    public void SetLocalType(LocaleTypes type)
    {
        localeType = type;
    }
}