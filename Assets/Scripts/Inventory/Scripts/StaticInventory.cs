using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StaticInventory : UserInterface
{
   
    public GameObject[] slots;
    

    public override void CreateInventorySlots()
    {
       _inventoryObjects=new Dictionary<GameObject, InventorySlot> ();
        for (int i = 0; i < inventory.GetItems.Length; i++)
        {
            var obj = slots[i];

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj);  });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });
            inventory.GetItems[i].slotDisplay = obj;
            _inventoryObjects.Add(obj, inventory.GetItems[i]);

        }
    }
}
