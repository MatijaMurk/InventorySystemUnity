using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UserInterface : MonoBehaviour
{

    public InventoryObject inventory;

    protected Dictionary<GameObject, InventorySlot> _inventoryObjects = new Dictionary<GameObject, InventorySlot>();

    
  
 
    void Awake()
    {
        
        for (int i = 0; i < inventory.GetItems.Length; i++)
        {
            inventory.GetItems[i].parent = this;
            inventory.GetItems[i].OnAfterUpdate += UpdateSlots;
        }
        CreateInventorySlots();

        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });

        gameObject.SetActive(false);

    }

    private void Update()
    {
        // _inventoryObjects.UpdateSlotDisplay();

        Debug.Log(MouseInfo.hoverInterface);
    }


    private void UpdateSlots(InventorySlot _slot)
    {
        if (_slot.item.ID >= 0)
        {
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.ItemObject.spriteUI;
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            _slot.slotDisplay.GetComponentInChildren<Text>().text = _slot.amount == 1 ? "" : _slot.amount.ToString("n0");
        }
        else
        {
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
            _slot.slotDisplay.GetComponentInChildren<Text>().text = "";
        }
    }
    public abstract void CreateInventorySlots();


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
        MouseInfo.hoveredSlot = obj;
    }
    public void OnExit(GameObject obj)
    {
        MouseInfo.hoveredSlot = null;  
    }
    public void OnEnterInterface(GameObject obj)
    {
        MouseInfo.hoverInterface = obj.GetComponent<UserInterface>();
    }
    public void OnExitInterface(GameObject obj)
    {
        MouseInfo.hoverInterface = null;
    }
    public void OnDragStart(GameObject obj)
    {
        
        if (MouseInfo.itemInAir == null)
        {
            MouseInfo.tempInfo = null;
            MouseInfo.tempInfo = new GameObject();
            MouseInfo.tempInfo = obj;
            MouseInfo.itemInAir = CreateTempItem(obj);
            //StartCoroutine(StartDrag());
        }
       //else OnDragEnd(obj);
        
      
    }
    public GameObject CreateTempItem(GameObject obj)
    {
        GameObject tempItem = null;
        
        if (_inventoryObjects[obj].item.ID >= 0)
        {
            tempItem = new GameObject();
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50);
            tempItem.transform.SetParent(transform.parent);
            var img = tempItem.AddComponent<Image>();
            img.sprite = _inventoryObjects[obj].ItemObject.spriteUI;
            img.raycastTarget = false;
        }
        return tempItem;
    }
    public void OnDrag(GameObject obj)
    {
        if (MouseInfo.itemInAir != null)
            MouseInfo.itemInAir.GetComponent<RectTransform>().position = Input.mousePosition;
    }
    public void OnDragEnd(GameObject obj)
    {
        
            Destroy(MouseInfo.itemInAir);
            if (MouseInfo.hoverInterface == null)
            {
                _inventoryObjects[obj].RemoveItem();
                return;
            }
            if (MouseInfo.hoveredSlot)
            {

                InventorySlot mouseHoverSlotData = MouseInfo.hoverInterface._inventoryObjects[MouseInfo.hoveredSlot];
                inventory.SwapItems(_inventoryObjects[obj], mouseHoverSlotData);
            }
        
        //if(MouseInfo.hoveredSlot==null && MouseInfo.hoverInterface != null)
        //{
        //    inventory.AddItem(, 1);
        //}
    }


    IEnumerator StartDrag()
    {
        while (MouseInfo.itemInAir)
        {
            MouseInfo.itemInAir.GetComponent<RectTransform>().position = Input.mousePosition;
            yield return null;
        }
        
    }

    void OnDisable()
    {
        Destroy(MouseInfo.itemInAir);
    }

    private void OnApplicationQuit()
    {
        inventory.Clear();
    }

}
public static class MouseInfo
{
    public static UserInterface hoverInterface;
    public static GameObject itemInAir;
    public static GameObject hoveredSlot;
    public static GameObject tempInfo;
}

public static class ExtensionMethods
{
    public static void UpdateSlotDisplay(this Dictionary<GameObject, InventorySlot> _slotsOnInterface)
    {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in _slotsOnInterface)
        {
            if (_slot.Value.item.ID >= 0)
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.Value.ItemObject.spriteUI;
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
}
