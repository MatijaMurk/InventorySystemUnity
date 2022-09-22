using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Potion,
    Helmet,
    Chest,
    Weapon,
    Shield,
    Boots,
    Default
}

public enum Attributes
{
    Agility,
    Stamina,
    Strength,
    Intelligence
}
public abstract class ItemObject : ScriptableObject
{
    public int ID;
    public Sprite spriteUI;
    public bool stackable;
    public ItemType type;
    [TextArea(15, 20)]
    public string description;
    public ItemBuff[] buffs;

    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }
}

[System.Serializable]
public class Item
{
    public string Name;
    public int ID;
    public ItemBuff[] buffs;
    public Item(ItemObject item)
    {
        Name = item.name;
        ID = item.ID;
        buffs = new ItemBuff[item.buffs.Length];
        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(item.buffs[i].min, item.buffs[i].max)
            {
                attribute = item.buffs[i].attribute
            };
        }
    }
}

[System.Serializable]
public class ItemBuff
{
    public Attributes attribute;
    public int value;
    public int min;
    public int max;
    public ItemBuff(int _min, int _max)
    {
        min = _min;
        max = _max;
        GenerateValue();
    }
    public void GenerateValue()
    {
        value = UnityEngine.Random.Range(min, max);
    }
}