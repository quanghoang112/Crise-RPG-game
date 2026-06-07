using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StorageSlot : UI_ItemSlot
{
    private InventoryStorage storage;

    public enum StorageSlotType { StorageSlot, PlayerInventorySlot}
    public StorageSlotType slotType;
    public void SetStorageForUiStorageSlot(InventoryStorage storage) => this.storage = storage;

    public override void OnPointerDown(PointerEventData eventData)
    {
        // base.OnPointerDown(eventData);

        if(itemInSlot == null)
            return;

        if(slotType == StorageSlotType.StorageSlot)
            storage.FromStorageToPlayer(itemInSlot);
        
        if(slotType == StorageSlotType.PlayerInventorySlot)
            storage.FromPlayerToStorage(itemInSlot);

            ui.itemToolTip.showToolTip(false, null);
    }
}
