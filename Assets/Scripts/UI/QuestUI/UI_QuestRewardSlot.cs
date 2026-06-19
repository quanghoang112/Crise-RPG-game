using UnityEngine;
using UnityEngine.EventSystems;

public class UI_QuestRewardSlot : UI_ItemSlot
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        // base.OnPointerDown(eventData);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        // base.OnPointerEnter(eventData);
        ui.itemToolTip.showToolTip(true,rect,itemInSlot,false,false);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        // base.OnPointerExit(eventData);
        ui.itemToolTip.showToolTip(false,null);
    }


}
