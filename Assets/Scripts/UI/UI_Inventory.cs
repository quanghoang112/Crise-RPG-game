using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private UI_ItemSlot[] uiItemSlots;
    private UI_EquipSlot[] uiEquipSlots;
    private InventoryPlayer inventory;

    [SerializeField] private Transform uiItemSlotParent;
    [SerializeField] private Transform uiEquipSlotParent;

    private void Awake()
    {
        uiItemSlots = uiItemSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        uiEquipSlots = uiEquipSlotParent.GetComponentsInChildren<UI_EquipSlot>();
        
        inventory = FindAnyObjectByType<InventoryPlayer>();
    
        inventory.OnInventoryChange += UpdateUI;

        UpdateUI();
    }

    private void UpdateUI()
    {
        UpdateInventorySlots();
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

    private void UpdateInventorySlots()
    {
        List<InventoryItem> ItemList = inventory.itemList; // số lượng item thực sự player sở hữu

        for(int i = 0; i<uiItemSlots.Length; i++) // vẽ item_ui lên các ô có sẵn
        {
            if(i<ItemList.Count)
            {
                uiItemSlots[i].UpdateSlot(ItemList[i]);
            }
            else
            {
                uiItemSlots[i].UpdateSlot(null);
            }
        }
    }

}
