using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapLocation))]
public abstract class Locale : MonoBehaviour
{
    protected Player player;
    private void Awake()
    {
        player = Player.Instance();
    }
    public abstract void Activate();
}