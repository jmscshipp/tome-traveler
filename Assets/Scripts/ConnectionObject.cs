using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// meant to be put on the graphic representation of a connection between two map locations
// at the moment this just manages fog of war stuff
public class ConnectionObject : MonoBehaviour
{
    [SerializeField]
    private GameObject FOWRevealer;

    public void RevealConnection(bool isVisible)
    {
        FOWRevealer.SetActive(isVisible);
    }
}
