using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_QuickItemSlotOption : UI_ItemSlot
{
    private UI_QuickItemSlot currentQuickItemSlot;



    public void SetupOption(UI_QuickItemSlot currentQuickItemSlot, InventoryItem itemToSet)
    {
        this.currentQuickItemSlot = currentQuickItemSlot;
        UpdateSlot(itemToSet);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        // base.OnPointerDown(eventData);
        currentQuickItemSlot.SetupQuickSlotItem(itemInSlot);
        ui.inGameUI.HideQuickItemOptions();
    }

}
