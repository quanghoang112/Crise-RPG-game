using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipSlot : UI_ItemSlot
{
    public ItemType slotType;

    private void OnValidate()
    {
        gameObject.name = "UI_EquipmentSlot - " + slotType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        // base.OnPointerDown(eventData);
        if(itemInSlot == null || itemInSlot.itemData.itemType == ItemType.Material)
            return;
        
        inventory.UnequipItem(itemInSlot);
        ui.itemToolTip.showToolTip(false,null);
    }
}
