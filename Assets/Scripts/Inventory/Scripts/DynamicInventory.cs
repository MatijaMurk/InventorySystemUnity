using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInventory : UserInterface
{
    public GameObject inventoryPrefab;
    [SerializeField]
    private RectTransform _contentPanel;

    
    public override void CreateInventorySlots()
    {
        _inventoryObjects = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.GetItems.Length; i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.transform.SetParent(_contentPanel);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            inventory.GetItems[i].slotDisplay = obj;


            _inventoryObjects.Add(obj, inventory.GetItems[i]);
        }


    }

 
}
