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
    public Inventory items;
    public int inventorySize=12;

    private void OnEnable()
    {
        items.NewSize(inventorySize);
    }

    public void AddItem(Item _item, int _amount)
    {

        if (_item.buffs.Length > 0)
        {
            SetEmptySlot(_item, _amount);
            return;
        }

        for (int i = 0; i < items.Items.Length; i++)
        {
            if (items.Items[i].ID == _item.ID)
            {
                items.Items[i].AddAmount(_amount);
                return;
            }
        }
        SetEmptySlot(_item, _amount);

    }
    public InventorySlot SetEmptySlot(Item _item, int _amount)
    {
        
        for (int i = 0; i < items.Items.Length; i++)
        {
            if (items.Items[i].ID <= -1)
            {
                items.Items[i].UpdateSlot(_item.ID, _item, _amount);
                return items.Items[i];
            }
        }
        //set up functionality for full inventory
        return null;
    }
    public void MoveItem(InventorySlot item1, InventorySlot item2)
    {
        InventorySlot temp = new InventorySlot(item2.ID, item2.item, item2.amount);
        item2.UpdateSlot(item1.ID, item1.item, item1.amount);
        item1.UpdateSlot(temp.ID, temp.item, temp.amount);
    }
    public void RemoveItem(Item _item)
    {
        for (int i = 0; i < items.Items.Length; i++)
        {
            if (items.Items[i].item == _item)
            {
                items.Items[i].UpdateSlot(-1, null, 0);
            }
        }
    }
    [ContextMenu("Save")]
    public void Save()
    {
        //string saveData= JsonUtility.ToJson(this,true);
        //BinaryFormatter binaryFormatter=new BinaryFormatter();
        //FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        //binaryFormatter.Serialize(file, saveData);
        //file.Close();
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, items);
        stream.Close();
    }
    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            //BinaryFormatter binaryFormatter = new BinaryFormatter();
            //FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            //JsonUtility.FromJsonOverwrite(binaryFormatter.Deserialize(file).ToString(), this);
            //file.Close();
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            items = (Inventory)formatter.Deserialize(stream);
            stream.Close();
        }
    }
    [ContextMenu("Clear")]
    public void Clear()
    {
        items = new Inventory();
        items.NewSize(inventorySize);
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
            //Items[i].RemoveItem();
        }
    }
}

[System.Serializable]
public class InventorySlot
{
    public ItemType[] AllowedItems = new ItemType[0];
    public UserInterface parent;
    public int ID=-1;
    public Item item;
    public int amount;
    public InventorySlot()
    {
        ID = -1;
        item = null;
        amount = 0;
    }
    public InventorySlot(int _id, Item _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }
    public void UpdateSlot(int _id, Item _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }
    public void AddAmount(int value)
    {
        amount += value;
    }
    public bool CanPlaceInSlot(ItemObject _item)
    {
        if (AllowedItems.Length <= 0)
        {
            return true;
        }
        for(int i = 0; i < AllowedItems.Length; i++)
        {
            if (_item.type == AllowedItems[i])
                return true;
        }
        return false;
    }
}
