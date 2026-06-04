using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private UI_EquipSlot[] uiEquipSlots;
    private InventoryPlayer inventory;

    [SerializeField] private UI_ItemSlotParent inventorySlotsParent;
    [SerializeField] private Transform uiEquipSlotParent;

    private void Awake()
    {
        uiEquipSlots = uiEquipSlotParent.GetComponentsInChildren<UI_EquipSlot>();
        
        inventory = FindAnyObjectByType<InventoryPlayer>();
    
        inventory.OnInventoryChange += UpdateUI;

        UpdateUI();
    }

    private void UpdateUI()
    {
        inventorySlotsParent.UpdateSlots(inventory.itemList);
        UpdateEquipmentSlots();
    }
    private void UpdateEquipmentSlots() // kiểm tra slot để vẽ UI
    {
        List<InventoryEquipmentSlots> playerEquipList = inventory.equipList;

        for(int i = 0;i<uiEquipSlots.Length;i++)
        {
            var playerEquipSlot = playerEquipList[i];

            if(playerEquipSlot.HasItem() == false)
            {
                uiEquipSlots[i].UpdateSlot();
            }
            else
                uiEquipSlots[i].UpdateSlot(playerEquipSlot.equipedItem);
        }
    }

}
