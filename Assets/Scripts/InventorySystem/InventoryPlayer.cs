using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPlayer : InventoryBase
{
    public event Action<int> OnQuickSlotUsed;
    
    public List<InventoryEquipmentSlots> equipList; //List chứa slot các trang bị (slot có thể có trang bị hoặc ko có trang bị) - logic
    public InventoryStorage storage {get;private set;}

    [Header("Quick Item Slots")]
    public InventoryItem[] quickItems = new InventoryItem[2];

    [Header("Gold info")]
    public int gold = 100000;
    // public bool isSave = false;


    protected override void Awake()
    {
        base.Awake();
        storage = FindAnyObjectByType<InventoryStorage>();
    }

    public void SetQuickItemInSlot(int slotNumber, InventoryItem itemToSet)
    {
        quickItems[slotNumber - 1] = itemToSet;
        OnTriggerUpdateUI();
    }

    public void TryUseQuickItemInSlot(int passedSlotNumber)
    {
        int slotNumber = passedSlotNumber - 1;
        var itemToUse = quickItems[slotNumber];

        if(itemToUse == null)
        {
            return;
        }

        TryUseItem(itemToUse);

        if(FindItem(itemToUse) == null) //Kiểm tra xem sau khi dùng thì còn món đồ đó không
        {
            quickItems[slotNumber] = FindSameItem(itemToUse); //không còn thì thử kiếm ở slot khác
        }
        OnTriggerUpdateUI();
        OnQuickSlotUsed?.Invoke(slotNumber);
    }

    public void TryEquipItem (InventoryItem item)
    {
        var inventoryItem = FindItem(item);
        var matchingSlots = equipList.FindAll(slot => slot.slotType == item.itemData.itemType);
        
        //Try to find empty slot and equip item
        foreach(var slot in matchingSlots)
        {
            if(slot.HasItem() == false) //slot trống thì trang bị vũ khí vào slot đó (xử lý logic)
            {
                EquipItem(inventoryItem, slot);
                return;
            }
        }

        //try to replace first slots finded
        var slotToReplace = matchingSlots[0];
        var itemToUnequip = slotToReplace.equipedItem;

        UnequipItem(itemToUnequip, slotToReplace != null);
        EquipItem(inventoryItem, slotToReplace);
    }

    private void EquipItem(InventoryItem itemToEquip, InventoryEquipmentSlots slot)
    {
        float saveHealthPercent = player.entityHealth.GetHealthPercent();
        
        slot.equipedItem = itemToEquip;
        slot.equipedItem.AddModifiers(player.entityStats);
        slot.equipedItem.AddItemEffect(player);

        player.entityHealth.SetHealthToPercent(saveHealthPercent);
        
        RemoveOneItem(itemToEquip);
    }

    public void UnequipItem (InventoryItem itemToUnequip, bool replacingItem = false)
    {
        if(CanAddItem(itemToUnequip) == false && !replacingItem)
        {
            Debug.Log("No space!");
            return;
        }

        float savedHealthPercent = player.entityHealth.GetHealthPercent();
        // Debug.Log(savedHealthPercent);

        var slotToUnequip = equipList.Find(slot => slot.equipedItem == itemToUnequip);

        if(slotToUnequip != null)
            slotToUnequip.equipedItem = null;

        itemToUnequip.RemoveModifiers(player.entityStats);
        itemToUnequip.RemoveItemEffect();
        
        player.entityHealth.SetHealthToPercent(savedHealthPercent);
        AddItem(itemToUnequip);
    }

    public override void SaveData(ref GameData data)
    {
        base.SaveData(ref data);

        // Debug.Log("Saving gold: " + gold);
        data.gold = gold;
        data.inventory.Clear();
        data.equipedItems.Clear();

        foreach (var item in itemList)
        {
            if (item != null && item.itemData != null)
            {
                string saveId = item.itemData.saveID;
                

                if (data.inventory.ContainsKey(saveId) == false)
                    data.inventory[saveId] = 0;

                data.inventory[saveId] += item.stackSize;
            }
        }

        foreach (var slot in equipList)
        {
            if (slot.HasItem())
            {
                //Đang bị lỗi nếu nhiều vật phẩm cùng loại thì sẽ bị ghi đè saveID dẫn đến load ra bị thiếu đồ, cần fix sau
                Debug.Log("Save id: " + slot.equipedItem.itemData.saveID + " Slot type: " + slot.slotType);
                data.equipedItems[slot.equipedItem.itemData.saveID] = slot.slotType;
            }
        }
    }

    public override void LoadData(GameData data)
    {
        base.LoadData(data);

        // Debug.Log(data.gold);
        // Debug.Log(gold);
        // Debug.Log(data == null);

        // gold = data.gold;

        gold = data.gold == -1 ? gold : data.gold;

        foreach (var entry in data.inventory)
        {
            string saveId = entry.Key;
            int stackSize = entry.Value;

            ItemDataSO itemData = itemDataBase.GetItemData(saveId);

            if (itemData == null)
            {
                Debug.LogWarning("Item not found: " + saveId);
                continue;
            }

            InventoryItem itemToLoad = new InventoryItem(itemData);

            for (int i = 0; i < stackSize; i++)
            {
                AddItem(itemToLoad);    
            }
        }

        foreach (var entry in data.equipedItems)
        {
            string saveId = entry.Key;
            ItemType equipemntSlotType = entry.Value;

            ItemDataSO itemData = itemDataBase.GetItemData(saveId);
            InventoryItem itemToLoad = new InventoryItem(itemData);

            var slot = equipList.Find(slot => slot.slotType == equipemntSlotType && slot.HasItem() == false);

            slot.equipedItem = itemToLoad;
            slot.equipedItem.AddModifiers(player.entityStats);
            slot.equipedItem.AddItemEffect(player);
        }

        OnTriggerUpdateUI();
    }
}
