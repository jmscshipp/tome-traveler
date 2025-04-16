using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemData", order = 1)]
public class ItemData : ScriptableObject
{
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private Items itemType;

    public Sprite Sprite
    {
        get { return sprite; }
    }

    public Items ItemType
    {
        get { return itemType; }
    }
}
