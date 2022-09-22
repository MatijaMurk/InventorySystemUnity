using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventoryObject inventory;
    [SerializeField] private GameObject inventoryScreen;
    [SerializeField] private GameObject equipScreen;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var item = collision.GetComponent<GroundItem>();
        if (item)
        {
            Item _item = new Item(item.item);
            
            inventory.AddItem(_item, 1);
            Destroy(collision.gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            inventory.Save();
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            inventory.Load();
        }
        if (Input.GetKeyDown(KeyCode.I)|| Input.GetKeyDown(KeyCode.Tab))
        {
            if (inventoryScreen.activeSelf)
            {
                inventoryScreen.SetActive(false);
            }
            else
            {
                inventoryScreen.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (equipScreen.activeSelf)
            {
                equipScreen.SetActive(false);
            }
            else
            {
                equipScreen.SetActive(true);
            }
        }
    }

        private void OnApplicationQuit()
    {
        inventory.items.Items = new InventorySlot[inventory.inventorySize];
    }
}
