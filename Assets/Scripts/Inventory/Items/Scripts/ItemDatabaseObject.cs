using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item Database", menuName ="Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] Items;
    public Dictionary<ItemObject, int> GetID;
    public Dictionary<int,ItemObject> GetItem;

    public void OnAfterDeserialize()
    {
        GetItem=new Dictionary<int,ItemObject>();  
       GetID=new Dictionary<ItemObject, int>();
        for(int i = 0; i < Items.Length; i++)
        {
            GetID.Add(Items[i], i);
            GetItem.Add(i, Items[i]);
        }
    }

    public void OnBeforeSerialize()
    {
       
    }
}
