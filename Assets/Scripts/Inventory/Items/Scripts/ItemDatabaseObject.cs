using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New GroundItem Database", menuName ="Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] Items;

    [ContextMenu("Update ID's")]
    public void UpdateID()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i].data.ID != i)
                Items[i].data.ID = i;
        }
    }
    public void OnAfterDeserialize()
    {
        UpdateID();
    }

    public void OnBeforeSerialize()
    {
    }
}
