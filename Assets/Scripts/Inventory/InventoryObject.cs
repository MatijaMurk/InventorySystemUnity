using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName ="Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> items=new List<InventorySlot>();

    public void AddItem(ItemObject _item, int _amount)
    {
        bool hasItem = false;
        for(int i = 0; i < items.Count; i++)
        {
            if (items[i].item == _item)
            {
                items[i].AddAmount(_amount);
                hasItem = true;
                break;
            }
        }
        if (!hasItem)
        {
            items.Add(new InventorySlot(_item, _amount));
        }
    }

}

[System.Serializable]
public class InventorySlot
{
    public ItemObject item;
    public int amount;
    public InventorySlot(ItemObject _item, int _amount)
    {
        this.item = _item;
        this.amount = _amount;
    }

    public void AddAmount(int _value)
    {
        amount += _value;
    }
}
