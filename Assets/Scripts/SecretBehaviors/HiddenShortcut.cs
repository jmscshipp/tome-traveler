using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenShortcut : SecretLocale
{
    [SerializeField]
    public MapLocation SecretDestination;
    [SerializeField]
    bool IsTwoWay = true;
    MapLocation loc;

    public override void Activate()
    {
        // we don't need to check null cuz MapLocation is required for SecretLocale
        loc.GetConnectedLocations().Add(SecretDestination);
        // only add the way back if the connection is two way
        // TODO: make it so one-way connections can go in either direction not just from the SecretLocale 
        if (IsTwoWay)
        {
            SecretDestination.GetConnectedLocations().Add(loc);
        }
        MapController.Instance().RevealSecretConnection(loc, SecretDestination);
        Debug.Log("Revealed Secret Connection");
    }
    // Start is called before the first frame update
    void Start()
    {
        loc = GetComponent<MapLocation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
