using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Potion Object", menuName = "Inventory System/Items/Potion GroundItem")]

public class PotionItem : ItemObject
{
    public void Awake()
    {
        type = ItemType.Potion;
    }
}
