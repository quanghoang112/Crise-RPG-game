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

    public int buyPrice{get;private set;}
    public float sellPrice {get; private set;}

    public InventoryItem(ItemDataSO itemData)
    {
        this.itemData = itemData;

        itemId = itemData.itemName +Guid.NewGuid();
        modifiers = EquipmentData()?.modifiers;
        itemEffect = itemData.itemEffect;
        buyPrice = itemData.itemPrice;
        sellPrice = itemData.itemPrice * .65f;
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
    public void RemoveItemEffect()
    {
        // if(itemEffect == null)
        //     return;
        itemEffect?.Unsubscribe();
    }

    private EquipmentDataSO EquipmentData()
    {
        if(itemData is EquipmentDataSO equipment)
            return equipment;
        
        return null;
    }

    public string GetItemInfo()
    {
        StringBuilder sb = new StringBuilder();
        
        if(itemData.itemType == ItemType.Material)
        {
            sb.AppendLine("");
            sb.AppendLine("Used for crafting.");
            
            sb.AppendLine("");
            sb.AppendLine("");
            return sb.ToString();
        }

        if(itemData.itemType == ItemType.Consumable)
        {
            sb.AppendLine("");
            sb.AppendLine(itemData.itemEffect.effectDescription);
            sb.AppendLine("");
            sb.AppendLine("");
            return sb.ToString();
        }
        

//item.itemData.itemType == ItemType.weapon,armor,trinket

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
            sb.AppendLine("");
            sb.AppendLine("");
            
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
