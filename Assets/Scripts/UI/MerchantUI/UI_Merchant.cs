using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Merchant : MonoBehaviour
{
    private InventoryPlayer inventory;
    private InventoryMerchant merchant;
    

    [SerializeField] private TextMeshProUGUI goldText;
    [Space]
    [SerializeField] private UI_ItemSlotParent merchantSlots;
    [SerializeField] private UI_ItemSlotParent inventorySlots;
    [SerializeField] private UI_EquipSlotParent equipSlots;
    public void SetupMerchantUI (InventoryMerchant merchant, InventoryPlayer inventory)
    {
        this.merchant = merchant;
        this.inventory = inventory;
    
        // merchant.OnInventoryChange += UpdateSlotUI; // lắng nghe invoke từ việc player mua item
        //tức là merchant bị mất item đó => merchant cũng phải lắng nghe invoke
        //tuy nhiên nếu ta cho quá trình mua, bán item sẽ là player invoke thì không cần merchant lắng nghe
        this.inventory.OnInventoryChange += UpdateSlotUI; // vì player đang mua, bán item (OnInventoryChange.Invoke được gọi ở các function mua bán)
        // , nên ta phải để hệ thống lắng nghe từ Player thay vì Merchant. Tuy nhiên chỉ update inventory của Player
        UpdateSlotUI();

        UI_MerchantSlot[] merchantSlots = GetComponentsInChildren<UI_MerchantSlot>();

        foreach (var slot in merchantSlots)
            slot.SetupMerchantUI(merchant);
    }

    private void UpdateSlotUI()
    {
        if(inventory == null)   return;
        
        inventorySlots.UpdateSlots(inventory.itemList);
        merchantSlots.UpdateSlots(merchant.itemList);
        equipSlots.UpdateEquipmentSlots(inventory.equipList);
    
        goldText.text = inventory.gold.ToString("N0") +"g.";
    }
}
