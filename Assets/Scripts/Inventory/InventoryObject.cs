using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName ="Inventory System/Inventory")]
public class InventoryObject : ScriptableObject, ISerializationCallbackReceiver
{
    public string savePath;
    private ItemDatabaseObject database;
    public List<InventorySlot> items=new List<InventorySlot>();

    private void OnEnable()
    {
#if UNITY_EDITOR
        database = (ItemDatabaseObject)AssetDatabase.LoadAssetAtPath("Assets/Resources/Data.asset", typeof(ItemDatabaseObject));
#else
        database = Resources.Load<ItemDatabaseObject>("Data");
#endif

    }
    public void AddItem(ItemObject _item, int _amount)
    {
       
        for(int i = 0; i < items.Count; i++)
        {
            if (items[i].item == _item)
            {
                items[i].AddAmount(_amount);
                return;
            }
        } 
        items.Add(new InventorySlot(database.GetID[_item],_item, _amount));
        
    }

    public void Save()
    {
        string saveData= JsonUtility.ToJson(this,true);
        BinaryFormatter binaryFormatter=new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        binaryFormatter.Serialize(file, saveData);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(binaryFormatter.Deserialize(file).ToString(), this);
            file.Close();
        }
    }

    public void OnAfterDeserialize()
    {
        for(int i = 0; i < items.Count; i++)
        {
            items[i].item = database.GetItem[items[i].ID];
        }
    }

    public void OnBeforeSerialize()
    {     
    }
}

[System.Serializable]
public class InventorySlot
{
    public int ID;
    public ItemObject item;
    public int amount;
    public InventorySlot(int _id, ItemObject _item, int _amount)
    {
        this.ID = _id;
        this.item = _item;
        this.amount = _amount;
    }

    public void AddAmount(int _value)
    {
        amount += _value;
    }
}
