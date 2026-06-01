using System;
using UnityEngine;



[Serializable]
public class InventoryItem
{
    private string itemId;
    public ItemDataSO itemData;
    public int stackSize = 1;

    public ItemModifier[] modifiers {get; private set;}


    public InventoryItem(ItemDataSO itemData)
    {
        this.itemData = itemData;

        modifiers = EquipmentData()?.modifiers;

        itemId = itemData.itemName +Guid.NewGuid();
    }

    public void AddModifiers(EntityStats playerStats)
    {
        foreach (var mod in modifiers)
        {
            Stat statToModify = playerStats.GetStatByType(mod.statsType);
            statToModify.AddModifier(mod.value,itemId);
        }
    }

    public void RemoveModifiers(EntityStats playerStats)
    {
        foreach (var mod in modifiers)
        {
            Stat statToModify = playerStats.GetStatByType(mod.statsType);
            statToModify.RemoveModifier(itemId);
        }
    }

    private EquipmentDataSO EquipmentData()
    {
        if(itemData is EquipmentDataSO equipment)
            return equipment;
        
        return null;
    }

    public bool CanAddStack() => stackSize < itemData.maxStackSize;
    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}
