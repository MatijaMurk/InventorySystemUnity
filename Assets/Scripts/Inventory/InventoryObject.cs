using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

public enum InterfaceType
{
    Inventory,
    Equipment,
    Chest
}

[CreateAssetMenu(fileName = "New Inventory", menuName ="Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public string savePath;
    public ItemDatabaseObject database;
    public InterfaceType type;
    public Inventory ItemContainer;
    public int inventorySize=12;
    
   
    public InventorySlot[] GetItems { get { return ItemContainer.Items; } }


    private void OnEnable()
    {
        ItemContainer.NewSize(inventorySize);
    }

    public bool AddItem(Item _item, int _amount)
    {
        if (database.Items[_item.ID].permanentUsage)
        {
            Debug.Log(database.Items[_item.ID].name + " applied");
            return true;
        }
        else
        {
            if (EmptySlotCount <= 0)
            {

                InventorySlot slot = FindItemOnInventory(_item);
                if (database.Items[_item.ID].stackable && slot != null)
                {
                    if (database.Items[_item.ID].maxStack > 0)
                    {
                        if (database.Items[_item.ID].maxStack > slot.amount)
                        {
                            slot.AddAmount(_amount);
                            return true;
                        }
                    }
                    else
                    {
                        slot.AddAmount(_amount);
                        return true;
                    }
                    return false;
                }
                else
                    return false;
            }

            else
            {
                InventorySlot slot = FindItemOnInventory(_item);

                if (!database.Items[_item.ID].stackable || slot == null)
                {

                    SetEmptySlot(_item, _amount);
                    return true;
                }

                slot.AddAmount(_amount);
                return true;
            }
        }
    }


    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < GetItems.Length; i++)
            {
                if (GetItems[i].item.ID <= -1)
                {
                    counter++;
                }
            }
            return counter;
        }
    }
    public InventorySlot FindItemOnInventory(Item _item)
    {
        for (int i = 0; i < GetItems.Length; i++)
        {
           
            if (GetItems[i].item.ID == _item.ID)
            {
                if (GetItems[i].ItemObject.maxStack > 0)
                {
                    if (GetItems[i].amount < GetItems[i].ItemObject.maxStack)
                        return GetItems[i];
                }
                else
                    return GetItems[i];  
            }
        }
        return null;
    }
    public InventorySlot SetEmptySlot(Item _item, int _amount)
    {
        
        for (int i = 0; i < GetItems.Length; i++)
        {
            if (GetItems[i].item.ID <= -1)
            {
                GetItems[i].UpdateSlot( _item, _amount);
                return GetItems[i];
            }
        }
        return null;
    }
    public void SwapItems(InventorySlot item1, InventorySlot item2)
    {
        
        if (item2.CanPlaceInSlot(item1.ItemObject) && item1.CanPlaceInSlot(item2.ItemObject))
        {
           
            InventorySlot temp = new InventorySlot(item2.item, item2.amount);
            item2.UpdateSlot(item1.item, item1.amount);
            item1.UpdateSlot(temp.item, temp.amount);
        }
    }
    public void RemoveItem(Item _item)
    {
        for (int i = 0; i < ItemContainer.Items.Length; i++)
        {
            if (ItemContainer.Items[i].item == _item)
            {
                ItemContainer.Items[i].UpdateSlot( null, 0);
            }
        }
    }
    [ContextMenu("Save")]
    public void Save()
    {
        
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, ItemContainer);
        stream.Close();
    }
    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Inventory newContainer = (Inventory)formatter.Deserialize(stream);
            for (int i = 0; i < GetItems.Length; i++)
            {
                GetItems[i].UpdateSlot(newContainer.Items[i].item, newContainer.Items[i].amount);
            }
            stream.Close();
        }
    }
    [ContextMenu("Clear")]
    public void Clear()
    {
        ItemContainer.Clear();
    }


}

[System.Serializable]
public class Inventory
{
    public InventorySlot[] Items= new InventorySlot[32];
    
    public void NewSize(int inventorySize)
    {
        Array.Resize(ref Items, inventorySize);
    }
    public void Clear()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            Items[i].RemoveItem();
        }
    }
}
public delegate void SlotUpdated(InventorySlot _slot);
[System.Serializable]
public class InventorySlot
{
    public ItemType[] AllowedItems = new ItemType[0];
    [System.NonSerialized]
    public UserInterface parent;
    [System.NonSerialized]
    public GameObject slotDisplay;
    [System.NonSerialized]
    public SlotUpdated OnAfterUpdate;
    [System.NonSerialized]
    public SlotUpdated OnBeforeUpdate;
    public Item item = new Item();
    public int amount;

    public PlayerModifications player;

    public ItemObject ItemObject
    {
        get
        {
            if (item.ID >= 0)
            {
                return parent.inventory.database.Items[item.ID];
            }
            return null;
        }
    }
  

    public InventorySlot()
    {
        UpdateSlot(new Item(), 0);
    }
    public InventorySlot(Item _item, int _amount)
    {
        UpdateSlot(_item, _amount);
    }
    public void UpdateSlot(Item _item, int _amount)
    {
        if (OnBeforeUpdate != null)
            OnBeforeUpdate.Invoke(this);
        item = _item;
        amount = _amount;
        if (OnAfterUpdate != null)
            OnAfterUpdate.Invoke(this);
    }
    public void RemoveItem()
    {
        UpdateSlot(new Item(), 0);
        
    }
   
    public void AddAmount(int value)
    {
        UpdateSlot(item, amount += value);
    }
    public bool CanPlaceInSlot(ItemObject _itemObject)
    {
        if (AllowedItems.Length <= 0 || _itemObject == null || _itemObject.data.ID < 0)
            return true;
        for (int i = 0; i < AllowedItems.Length; i++)
        {
            if (_itemObject.type == AllowedItems[i])
                return true;
        }
        return false;
    }
}
