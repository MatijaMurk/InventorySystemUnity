using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Potion,
    Equipment,
    Default
}

public enum Attributes
{
    Agility,
    Magic,
    Strength,
    Stamina,
    Luck
}

public abstract class ItemObject : ScriptableObject
{
    public int ID;
    public Sprite spriteUI;
    public ItemType type;

    [TextArea(20,25)]
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
    public string name;
    public int ID;
    public ItemBuff[] buffs;
    public Item(ItemObject item)
    {
        name=item.name;
        ID=item.ID;
        buffs = new ItemBuff[item.buffs.Length];
        for(int i=0; i<buffs.Length; i++)
        {
            buffs[i]=new ItemBuff(item.buffs[i].min, item.buffs[i].max);
            {
                buffs[i].attribute = item.buffs[i].attribute;
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
        value=UnityEngine.Random.Range(min,max);    
    }
}
