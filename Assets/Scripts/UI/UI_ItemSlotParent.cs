using System.Collections.Generic;
using UnityEngine;

public class UI_ItemSlotParent : MonoBehaviour
{
    private UI_ItemSlot[] slots;

    public void UpdateSlots(List<InventoryItem> itemList)
    {
        if(slots == null)
            slots = GetComponentsInChildren<UI_ItemSlot>();

        for(int i = 0; i<slots.Length; i++) // vẽ item_ui lên các ô có sẵn
        {
            if(i<itemList.Count)
            {
                slots[i].UpdateSlot(itemList[i]);
            }
            else
            {
                slots[i].UpdateSlot(null);
            }
        }
    }
}
