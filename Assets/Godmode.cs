using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Godmode : MonoBehaviour
{
    GameObject indicator;   
    // Start is called before the first frame update
    void Start()
    {
        indicator = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        #if UNITY_EDITOR
            // this doesn't go into any standalone build
            indicator.SetActive(Player.Instance().godmode);
        #endif
    }
}
