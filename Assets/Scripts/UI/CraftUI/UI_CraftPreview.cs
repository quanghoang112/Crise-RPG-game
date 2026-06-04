// using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftPreview : MonoBehaviour
{
    private InventoryItem itemToCraft;
    private InventoryStorage storage;
    private UI_CraftPreviewSlot[] craftPreviewSlots;

    [Header("item preview Setup")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemInfo;
    [SerializeField] private TextMeshProUGUI buttonText;
    public void SetupCraftPreview(InventoryStorage storage)
    {
        this.storage = storage;

        craftPreviewSlots = GetComponentsInChildren<UI_CraftPreviewSlot>(true);
        foreach(var slot in craftPreviewSlots)
            slot.gameObject.SetActive(false);
    }

    public void ConfirmCraft()
    {
        if(itemToCraft == null)
        {
            buttonText.text = "Pick an item.";
            return;
        }

        if(storage.HasEnoughMaterials(itemToCraft) && storage.playerInventory.CanAddItem(itemToCraft))
        {
            storage.ConsumeMaterials(itemToCraft);
            storage.playerInventory.AddItem(itemToCraft);
        }

        UpdateCraftPreviewSlots();
    }

    // chỉ setupCraftPreview khi nhấn L
    public void UpdateCraftPreview(ItemDataSO itemData)
    {
        itemToCraft = new InventoryItem(itemData);
        
        itemIcon.sprite = itemToCraft.itemData.itemIcon;
        itemName.text = itemToCraft.itemData.itemName;
        itemInfo.text = itemToCraft.GetItemInfo();
    
        UpdateCraftPreviewSlots();
    }
    
    public void UpdateCraftPreviewSlots()
    {
        foreach (var slot in craftPreviewSlots)
            slot.gameObject.SetActive(false);

        for (int i = 0;i< itemToCraft.itemData.craftRecipe.Length;i++)
        {
            InventoryItem requiredItem = itemToCraft.itemData.craftRecipe[i];
            int avaliableAmount = storage.GetAvailableAmountOf(requiredItem.itemData);
            int requiredAmount = itemToCraft.stackSize;


            craftPreviewSlots[i].gameObject.SetActive(true);
            craftPreviewSlots[i].SetupMaterialSlot(requiredItem.itemData, avaliableAmount, requiredAmount);
        }
    }
}
