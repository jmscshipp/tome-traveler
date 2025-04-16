using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapLocation))]
public abstract class Locale : MonoBehaviour
{
    public abstract void Activate();
}