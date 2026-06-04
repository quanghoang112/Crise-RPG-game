using System.Collections.Generic;
using UnityEngine;

public class InventoryPlayer : InventoryBase
{
    private Player player;
    public List<InventoryEquipmentSlots> equipList; //List chứa slot các trang bị (slot có thể có trang bị hoặc ko có trang bị) - logic
    public InventoryStorage storage {get;private set;}
    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
        storage = FindAnyObjectByType<InventoryStorage>();
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

        var slotToUnequip = equipList.Find(slot => slot.equipedItem == itemToUnequip);

        if(slotToUnequip != null)
            slotToUnequip.equipedItem = null;

        itemToUnequip.RemoveModifiers(player.entityStats);
        itemToUnequip.RemoveItemEffect();
        
        player.entityHealth.SetHealthToPercent(savedHealthPercent);
        AddItem(itemToUnequip);
    }
}
