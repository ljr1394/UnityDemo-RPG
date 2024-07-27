using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler
{

    public Image itemImage;
    public TextMeshProUGUI itemAmount;

    public InventoryItem item;

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item.itemDate.itemType == ItemType.Equipment)
        {
            Inventory.instance.EqupimentItem(item.itemDate);
        }

    }

    public void UpdateSolt(InventoryItem _newItem)
    {
        item = _newItem;

        if (item != null)
        {
            itemImage.sprite = item.itemDate.icon;
            itemImage.color = Color.white;
            if (item.stackSize > 1)
            {
                itemAmount.text = item.stackSize.ToString();
            }
            else
            {
                itemAmount.text = "";
            }

        }
    }

    public void ClearSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemAmount.text = "";
    }

}
