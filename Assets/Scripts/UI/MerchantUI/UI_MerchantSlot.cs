using UnityEngine;
using UnityEngine.EventSystems;

public class UI_MerchantSlot : UI_ItemSlot
{
    private InventoryMerchant merchant;
    public enum MerchantSlotType{MerchantSlot, playerSlot};
    public MerchantSlotType slotType;

    public override void OnPointerDown(PointerEventData eventData)
    {
        // base.OnPointerDown(eventData);

        if(itemInSlot == null)
            return;
        
        bool rightButton = eventData.button == PointerEventData.InputButton.Right;
        bool leftButton = eventData.button == PointerEventData.InputButton.Left;

        if(slotType == MerchantSlotType.playerSlot)
        {
            if(rightButton)// sell item
            {
                bool sellFullStack = false;
                merchant.TrySellItem(itemInSlot,sellFullStack);
            }
            else if(leftButton)
            {
                //buy item

                base.OnPointerDown(eventData);// add item
            }
        }
        else if (slotType == MerchantSlotType.MerchantSlot)
        {
            if(leftButton)
                return;
            
            //buy item on merchant class

            bool buyFullStack = false;
            merchant.TryBuyItem(itemInSlot,buyFullStack);
        }

        ui.itemToolTip.showToolTip(false, null);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        // base.OnPointerEnter(eventData);

        if(itemInSlot == null) return;

        if(slotType == MerchantSlotType.MerchantSlot)
            ui.itemToolTip.showToolTip(true,rect,itemInSlot,true,true);
        else
            ui.itemToolTip.showToolTip(true,rect,itemInSlot,false, true);
    }
    public void SetupMerchantUI(InventoryMerchant merchant) => this.merchant = merchant;
}
