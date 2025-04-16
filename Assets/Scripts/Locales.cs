using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum LocaleTypes : ushort
{
    None = 0,
    Town,
    Wilds,
    Shop,
    City,
    Cabin,
    Ruins
}

[RequireComponent(typeof(MapLocation))]
public abstract class Locale : MonoBehaviour
{
    [SerializeField]
    protected string localeDescription;
    protected LocaleTypes localeType;

    public abstract void Activate();

    public abstract void Deactivate();

    public string GetLocaleDescription() => localeDescription;
    public LocaleTypes GetLocaleType() => localeType;
}