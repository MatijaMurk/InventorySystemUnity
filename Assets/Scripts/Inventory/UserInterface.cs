using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UserInterface : MonoBehaviour
{
    public MouseItem mouseItem = new MouseItem();
    public InventoryObject inventory;
    
    

    protected Dictionary<GameObject, InventorySlot> _inventoryObjects = new Dictionary<GameObject, InventorySlot>();



    void Start()
    {
        for(int i = 0; i < inventory.items.Items.Length; i++)
        {
            inventory.items.Items[i].parent = this;
        }
        CreateInventorySlots();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSlots();
    }

    public abstract void CreateInventorySlots();
  
    public void UpdateSlots()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in _inventoryObjects)
        {
            if (_slot.Value.ID >= 0)
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[_slot.Value.item.ID].spriteUI;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<Text>().text = _slot.Value.amount == 1 ? "" : _slot.Value.amount.ToString("n0");
            }
            else
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                _slot.Key.GetComponentInChildren<Text>().text = "";
            }
        }
    }


    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry
        {
            eventID = type
        };
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        mouseItem.hoverObj = obj;
        if (_inventoryObjects.ContainsKey(obj))
            mouseItem.hoverItem = _inventoryObjects[obj];
    }
    public void OnExit(GameObject obj)
    {

        mouseItem.hoverObj = null;
        mouseItem.hoverItem = null;
    }
    public void OnDragStart(GameObject obj)
    {
        var mouseObject = new GameObject();
        var rt = mouseObject.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(50, 50);
        mouseObject.transform.SetParent(transform.parent);
        if (_inventoryObjects[obj].ID >= 0)
        {
            var img = mouseObject.AddComponent<Image>();
            img.sprite = inventory.database.GetItem[_inventoryObjects[obj].ID].spriteUI;
            img.raycastTarget = false;
        }
        mouseItem.obj = mouseObject;
        mouseItem.item = _inventoryObjects[obj];
    }
    public void OnDragEnd(GameObject obj)
    {
      

        if (mouseItem.hoverObj)
        {
            inventory.MoveItem(_inventoryObjects[obj],mouseItem.hoverItem.parent._inventoryObjects[mouseItem.hoverObj]);
        }
        else
        {
            inventory.RemoveItem(_inventoryObjects[obj].item);
        }
        Destroy(mouseItem.obj);
        mouseItem.item = null;
    }
    public void OnDrag(GameObject obj)
    {
        if (mouseItem.obj != null)
        {
            mouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }


    void OnDisable()
    {
        Destroy(mouseItem.obj);
        mouseItem.item = null;
    }

    public class MouseItem
    {
        public GameObject obj;
        public InventorySlot item;
        public InventorySlot hoverItem;
        public GameObject hoverObj;
    }

}
