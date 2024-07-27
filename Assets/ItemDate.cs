using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Material,
    Equipment
}


[CreateAssetMenu(fileName = "New Item Date", menuName = "Date/Item", order = 0)]
public class ItemDate : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
    public Sprite image;
}

