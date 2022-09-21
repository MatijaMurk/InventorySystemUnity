using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventory;

    [SerializeField]
    private RectTransform _contentPanel;

    private Dictionary<InventorySlot,GameObject> inventoryObjects;

    private void Awake()
    {
        inventoryObjects = new Dictionary<InventorySlot, GameObject>();
    }

    void Start()
    {
        CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        for (int i = 0; i < inventory.items.Count; i++)
        {
            if (inventoryObjects.ContainsKey(inventory.items[i]))
            {
                inventoryObjects[inventory.items[i]].GetComponentInChildren<Text>().text = inventory.items[i].amount.ToString("n0");
            }
            else
            {
                var obj = Instantiate(inventory.items[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
                obj.transform.SetParent(_contentPanel);
                inventoryObjects.Add(inventory.items[i],obj);
            }

        }
    }
    public void CreateDisplay()
    {
        for (int i = 0; i < inventory.items.Count; i++)
        {
            
            var obj=Instantiate(inventory.items[i].item.prefab, Vector3.zero,Quaternion.identity,transform);
            obj.transform.SetParent(_contentPanel);
            inventoryObjects.Add(inventory.items[i], obj);
            
        }
    }
}
