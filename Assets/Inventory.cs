using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<InventoryItem> inventoryItems;
    public List<InventoryItem> stashItems;
    public List<InventoryItem> equpimentItems;
    public Dictionary<ItemDate, InventoryItem> inventoryDict;
    public Dictionary<ItemDate, InventoryItem> stashDict;
    public Dictionary<ItemDate_Equipment, InventoryItem> equipmentDict;

    [Header("Inventory UI")]
    [SerializeField] private Transform InventorySlotPrent;
    [SerializeField] private Transform stashSlotPrent;
    [SerializeField] private Transform equpimentSlotPrent;

    private UI_ItemSlot[] InventoryItemSlots;
    private UI_ItemSlot[] stashItemSlots;
    private UI_EqupimentSlot[] equpimentSlots;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        inventoryItems = new List<InventoryItem>();
        inventoryDict = new Dictionary<ItemDate, InventoryItem>();

        stashItems = new List<InventoryItem>();
        stashDict = new Dictionary<ItemDate, InventoryItem>();

        equipmentDict = new Dictionary<ItemDate_Equipment, InventoryItem>();
        equpimentItems = new List<InventoryItem>();

        InventoryItemSlots = InventorySlotPrent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlots = stashSlotPrent.GetComponentsInChildren<UI_ItemSlot>();
        equpimentSlots = equpimentSlotPrent.GetComponentsInChildren<UI_EqupimentSlot>();
    }

    public void UpdateInventoryUI()
    {

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            InventoryItemSlots[i].UpdateSolt(inventoryItems[i]);
        }
        for (int i = 0; i < stashItems.Count; i++)
        {
            stashItemSlots[i].UpdateSolt(stashItems[i]);

        }
        for (int i = 0; i < equpimentItems.Count; i++)
        {
            ItemDate_Equipment equipment = equpimentItems[i].itemDate as ItemDate_Equipment;
            foreach (var equpimentSlot in equpimentSlots)
            {
                if (equpimentSlot.slotType == equipment.equipmentType)
                {
                    equpimentSlot.UpdateSolt(equpimentItems[i]);
                    break;
                }
            }
        }
    }
    public void AddItem(ItemDate _item)
    {
        if (_item.itemType == ItemType.Equipment)
            AddToInventory(_item);
        else if (_item.itemType == ItemType.Material)
        {
            AddToStash(_item);
        }
        UpdateInventoryUI();
    }
    public void EqupimentItem(ItemDate _item)
    {
        ItemDate_Equipment newEqupiment = _item as ItemDate_Equipment;
        ItemDate_Equipment oldEqupiment = null;
        foreach (KeyValuePair<ItemDate_Equipment, InventoryItem> nowEqupiment in equipmentDict)
        {
            if (nowEqupiment.Key.equipmentType == newEqupiment.equipmentType)
            {
                oldEqupiment = nowEqupiment.Key;
                break;
            }
        }
        if (oldEqupiment != null)
        {
            UnequipItem(oldEqupiment);
            AddItem(oldEqupiment);
        }
        newEqupiment.AddModifiers();
        AddToEqupiment(newEqupiment);
        RemoveItem(newEqupiment);

    }

    public void UnequipItem(ItemDate_Equipment _removeEqument)
    {
        if (equipmentDict.TryGetValue(_removeEqument, out InventoryItem value))
        {
            equpimentItems.Remove(value);
            equipmentDict.Remove(_removeEqument);
            _removeEqument.RemoveModifiers();
        }


    }


    private void AddToInventory(ItemDate _item)
    {

        if (inventoryDict.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventoryDict.Add(_item, newItem);
            inventoryItems.Add(newItem);

        }
    }
    private void AddToEqupiment(ItemDate_Equipment _item)
    {
        InventoryItem newItem = new InventoryItem(_item);
        equipmentDict.Add(_item, newItem);
        equpimentItems.Add(newItem);
    }

    private void AddToStash(ItemDate _item)
    {
        if (stashDict.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stashDict.Add(_item, newItem);
            stashItems.Add(newItem);

        }
    }

    public void RemoveItem(ItemDate _item)
    {
        if (inventoryDict.TryGetValue(_item, out InventoryItem InventoryValue))
        {

            if (InventoryValue.stackSize <= 1)
            {

                ClearSlot(InventoryItemSlots, InventoryValue);
                inventoryItems.Remove(InventoryValue);

                inventoryDict.Remove(_item);


            }
            else
            {
                InventoryValue.RemoveStack();
            }

        }
        if (stashDict.TryGetValue(_item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {

                ClearSlot(stashItemSlots, stashValue);
                stashItems.Remove(stashValue);
                stashDict.Remove(_item);
            }
            else
            {
                stashValue.RemoveStack();
            }

        }
        UpdateInventoryUI();
    }

    public void ClearSlot(UI_ItemSlot[] _slots, InventoryItem _item)
    {

        Debug.Log(_slots.Length);
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].item == null)
            {
                return;
            }
            _slots[i].ClearSlot();


        }
    }


    void Update()
    {

    }
}
