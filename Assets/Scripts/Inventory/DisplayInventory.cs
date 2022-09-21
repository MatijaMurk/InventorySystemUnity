using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventory;
    public GameObject inventoryPrefab;
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
        for (int i = 0; i < inventory.items.Items.Count; i++)
        {
            InventorySlot slot =inventory.items.Items[i]; 
            if (inventoryObjects.ContainsKey(slot))
            {
                inventoryObjects[slot].GetComponentInChildren<Text>().text = slot.amount.ToString("n0");
            }
            else
            {
                var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
                obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[slot.item.ID].spriteUI;
                obj.transform.SetParent(_contentPanel);
                obj.GetComponentInChildren<Text>().text = slot.amount.ToString("n0");
                inventoryObjects.Add(slot, obj);
            }

        }
    }
    public void CreateDisplay()
    {
        for (int i = 0; i < inventory.items.Items.Count; i++)
        {
            InventorySlot slot = inventory.items.Items[i];
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[slot.item.ID].spriteUI;
            obj.transform.SetParent(_contentPanel);
            obj.GetComponentInChildren<Text>().text = slot.amount.ToString("n0");
            inventoryObjects.Add(slot, obj);

        }
    }
}
