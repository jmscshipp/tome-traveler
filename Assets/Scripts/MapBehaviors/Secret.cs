using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapLocation))]
public class Secret : MonoBehaviour
{
    [SerializeField]
    public MapLocation destination;
    [SerializeField]
    public string tooltop;
    public void Activate()
    {

    }
}
