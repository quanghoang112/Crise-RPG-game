using System;
using UnityEngine;
using System.Text;



[Serializable]
public class InventoryItem
{
    public string itemId;
    public ItemDataSO itemData;
    public int stackSize = 1;

    public ItemModifier[] modifiers {get; private set;}
    public ItemEffectDataSO itemEffect;

    public InventoryItem(ItemDataSO itemData)
    {
        this.itemData = itemData;

        modifiers = EquipmentData()?.modifiers;
        itemEffect = itemData.itemEffect;

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

    public void AddItemEffect(Player player) => itemEffect?.Subscribe(player);
    public void RemoveItemEffect() => itemEffect.Unsubscribe();

    private EquipmentDataSO EquipmentData()
    {
        if(itemData is EquipmentDataSO equipment)
            return equipment;
        
        return null;
    }

    public string GetItemInfo()
    {
        if(itemData.itemType == ItemType.Material)
            return "Used for crafting.";

        if(itemData.itemType == ItemType.Consumable)
            return itemData.itemEffect.effectDescription;

        

//item.itemData.itemType == ItemType.weapon,armor,trinket
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("");

        foreach (var mod in modifiers)
        {
            string modType = GetStatNameByType(mod.statsType);
            string signValue = mod.value > 0? mod.value.ToString():mod.value.ToString().Substring(1);
            string modValue = IsPercentageStat(mod.statsType) ? signValue + "%" : signValue;
            string mathSign = mod.value > 0? "+" : "-";
            sb.AppendLine($"{mathSign} {modValue} {modType}");
        }


        if(itemEffect != null)
        {
            sb.AppendLine("");
            sb.AppendLine("Unique effect:");
            sb.AppendLine(itemEffect.effectDescription);
        }

        return sb.ToString();
    }

    private string GetStatNameByType(StatsType type)
    {
        switch(type)
        {
            case StatsType.maxHealth: return "Max Health";
            case StatsType.healthRegen: return "Health Regenation";
            
            case StatsType.Strength: return "Strength";
            case StatsType.Agility: return "Agility";
            case StatsType.Vitality: return "Vitality";
            case StatsType.Intelligence: return "Intelligence";

            case StatsType.attackSpeed: return "Attack speed";
            case StatsType.damage: return "Damage";
            case StatsType.critChance: return "Crit Chance";
            case StatsType.critPower: return "Crit Power";
            case StatsType.armorReduction: return "Armor Reduction";

            case StatsType.fireDamage: return "Fire Damage";
            case StatsType.iceDamage: return "Ice Damage";
            case StatsType.lightningDamage: return "Lightning Damage";

            case StatsType.armor: return "Armor";
            case StatsType.evasion: return "Evasion";

            case StatsType.iceResistance: return "Ice Resistance";
            case StatsType.fireResistance: return "Fire Resistance";
            case StatsType.lightningResistance: return "Lightning Resistance";

            default:
                Debug.LogWarning($"StatsType {type} not implemented yet!");
                return null;
        }
    }

    private bool IsPercentageStat (StatsType type)
    {
        switch(type)
        {
            case StatsType.critChance:
            case StatsType.critPower:
            case StatsType.armorReduction:
            case StatsType.iceResistance:
            case StatsType.fireResistance:
            case StatsType.lightningResistance:
            case StatsType.attackSpeed:
                return true;
            default:
                return false;
        }
    }

    public bool CanAddStack() => stackSize < itemData.maxStackSize;
    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}
