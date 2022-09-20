using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Object", menuName = "Inventory System/Items/Equipment Item")]
public class EquipmentItem : ItemObject
{
    public int attackBonus;
    public int defenseBonus;
    public void Awake()
    {
        type = ItemType.Equipment;
    }
}
