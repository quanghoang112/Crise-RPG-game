using System;
using UnityEngine;
[Serializable]
public class InventoryEquipmentSlots
{
    public ItemType slotType;
    public InventoryItem equipedItem;

    public bool HasItem() => equipedItem != null && equipedItem.itemData != null;
}
