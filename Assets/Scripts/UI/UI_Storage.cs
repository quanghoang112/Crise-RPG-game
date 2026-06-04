using UnityEngine;

public class UI_Storage : MonoBehaviour
{
    private InventoryStorage storage;
    private InventoryPlayer playerInventory;

    [SerializeField] private UI_ItemSlotParent inventoryParent;
    [SerializeField] private UI_ItemSlotParent storageParent;
    [SerializeField] private UI_ItemSlotParent materialStashParent;
    public void SetupStorage(InventoryStorage storage)
    {
        this.storage = storage;
        this.playerInventory = storage.playerInventory;
        storage.OnInventoryChange += UpdateUI;
        UpdateUI();

        UI_StorageSlot[] storageSlots = GetComponentsInChildren<UI_StorageSlot>();

        foreach (var slot in storageSlots)
            slot.SetStorageForUiStorageSlot(storage);
    }

    private void UpdateUI()
    {
        inventoryParent.UpdateSlots(playerInventory.itemList);
        storageParent.UpdateSlots(storage.itemList);
        materialStashParent.UpdateSlots(storage.materialStash);
    }
}
