using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Object", menuName = "Inventory System/Items/Equipment GroundItem")]

public class EquipmentItem : ItemObject
{
    public void Awake()
    {
        type = ItemType.Chest;
    }
}
