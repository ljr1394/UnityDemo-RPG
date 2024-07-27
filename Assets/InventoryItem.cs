using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public ItemDate itemDate;

    public int stackSize;

    public InventoryItem(ItemDate _itemDate)
    {
        itemDate = _itemDate;
        AddStack();
    }

    public void AddStack()
    {
        stackSize++;
    }
    public void RemoveStack()
    {
        stackSize--;
    }
}
