using System.Collections.Generic;
using UnityEngine;

public class InventoryPlayer : InventoryBase
{
    private EntityStats playerStats;
    public List<InventoryEquipmentSlots> equipList; //List chứa slot các trang bị (slot có thể có trang bị hoặc ko có trang bị) - logic

    protected override void Awake()
    {
        base.Awake();
        playerStats = GetComponent<EntityStats>();
    }

    public void TryEquipItem (InventoryItem item)
    {
        var inventoryItem = FindItem(item.itemData);
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

        EquipItem(inventoryItem, slotToReplace);
        UnequipItem(itemToUnequip);
    }

    private void EquipItem(InventoryItem itemToEquip, InventoryEquipmentSlots slot)
    {
        slot.equipedItem = itemToEquip;
        slot.equipedItem.AddModifiers(playerStats);

        RemoveItem(itemToEquip);
    }

    public void UnequipItem (InventoryItem itemToUnequip)
    {
        if(CanAddItem() == false)
        {
            Debug.Log("No space!");
            return;
        }

        foreach (var slot in equipList)
        {
            if(slot.equipedItem == itemToUnequip)
            {
                slot.equipedItem = null;
                break;
            }
        }
        itemToUnequip.RemoveModifiers(playerStats);
        AddItem(itemToUnequip);
    }
}
