using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Potion Object", menuName = "Inventory System/Items/Potion Item")]

public class PotionItem : ItemObject
{

    public int healthAmount;
    public void Awake()
    {
        type = ItemType.Potion;
    }
}
