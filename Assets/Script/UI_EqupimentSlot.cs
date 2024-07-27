using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EqupimentSlot : UI_ItemSlot
{
    public EquipmentType slotType;






    public override void OnPointerDown(PointerEventData eventData)
    {
        Inventory.instance.UnequipItem(item.itemDate as ItemDate_Equipment);
        Inventory.instance.AddItem(item.itemDate);
        ClearSlot();
    }
}
