
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerModifications : MonoBehaviour
{
    public InventoryObject inventory;
    public InventoryObject equipment;
    [SerializeField] private GameObject inventoryScreen;
    [SerializeField] private GameObject equipScreen;
    [SerializeField] private DisplayStats statsScreen;

    [SerializeField] private GroundItem[] itemsToSpawn;
    private int itemCounter=0;
   

    public Attribute[] attributes;


    private void Start()
    {
        for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i].SetParent(this);
        }
        for (int i = 0; i < equipment.GetItems.Length; i++)
        {
            equipment.GetItems[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            equipment.GetItems[i].OnAfterUpdate += OnAfterSlotUpdate;
        }
    }
    public void OnBeforeSlotUpdate(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
            return;
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Removed ", _slot.ItemObject, " on ", _slot.parent.inventory.type, ", Allowed Items: ", string.Join(", ", _slot.AllowedItems)));

                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                        {
                            attributes[j].value.RemoveModifier(_slot.item.buffs[i]);
                            statsScreen.UpdateStatsText();
                        }
                            
                    }
                }

                break;
            case InterfaceType.Chest:
                break;
            default:
                break;
        }
    }
    public void OnAfterSlotUpdate(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
            return;
        switch (_slot.parent.inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Placed ", _slot.ItemObject, " on ", _slot.parent.inventory.type, ", Allowed Items: ", string.Join(", ", _slot.AllowedItems)));

                for (int i = 0; i < _slot.item.buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.buffs[i].attribute)
                        {
                            attributes[j].value.AddModifier(_slot.item.buffs[i]);
                            statsScreen.UpdateStatsText();
                        }
                           
                    }
                }

                break;
            case InterfaceType.Chest:
                break;
            default:
                break;
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        var item = collision.GetComponent<GroundItem>();
        if (item)
        {
            Item _item = new Item(item.item);

            if (inventory.AddItem(_item, 1))
            {
                Destroy(collision.gameObject);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            inventory.Save();
            equipment.Save();
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            inventory.Load();
            equipment.Load();
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
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (statsScreen.gameObject.activeSelf)
            {
                statsScreen.gameObject.SetActive(false);
            }
            else
            {
                statsScreen.gameObject.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnItems();
        }
    }

    public void SpawnItems()
    {
        
        if (itemCounter < itemsToSpawn.Length)
        {
            var item = Instantiate(itemsToSpawn[itemCounter], new Vector3(transform.position.x+ 3, transform.position.y-2, transform.position.z), Quaternion.identity);

            itemCounter++;
        }
        else
        {
            itemCounter = 0;
            var item = Instantiate(itemsToSpawn[itemCounter], new Vector3(transform.position.x + 3, transform.position.y - 2, transform.position.z), Quaternion.identity);
            itemCounter++;
        }
       

    }
    public void AttributeModified(Attribute attribute)
    {
        
        
        //Debug.Log(string.Concat(attribute.type, " was updated! Value is now ", attribute.value.ModifiedValue));
    }

  
}
[System.Serializable]
public class Attribute
{
    [System.NonSerialized]
    public PlayerModifications parent;
    public Attributes type;
    public ModifiableInt value;

    public void SetParent(PlayerModifications _parent)
    {
        parent = _parent;
        value = new ModifiableInt(AttributeModified);
    }
    public void AttributeModified()
    {
        parent.AttributeModified(this);
    }
}