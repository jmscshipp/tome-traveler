using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapLocation))]
public abstract class Locale : MonoBehaviour
{
    public Player player;
    private void Awake()
    {
        player = Player.Instance();
    }
    public abstract void Activate();
}