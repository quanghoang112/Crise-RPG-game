using System;
using UnityEngine;



[Serializable]
public class InventoryItem
{
    public ItemDataSo itemData;

    public InventoryItem(ItemDataSo itemData)
    {
        this.itemData = itemData;
    }
}
