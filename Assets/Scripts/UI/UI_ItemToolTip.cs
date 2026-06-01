using System.Text;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemInfo;

    public void showToolTip(bool show, RectTransform targetRect, InventoryItem itemToShow)
    {
        base.showToolTip(show, targetRect);

        itemName.text = itemToShow.itemData.itemName;
        itemType.text = itemToShow.itemData.itemType.ToString();
        itemInfo.text = GetItemInfo(itemToShow);
    }

    public string GetItemInfo(InventoryItem item)
    {
        if(item.itemData.itemType == ItemType.Material)
            return "Used for crafting.";

        StringBuilder sb = new StringBuilder();

        sb.AppendLine("");

        foreach (var mod in item.modifiers)
        {
            string modType = GetStatNameByType(mod.statsType);
            string signValue = mod.value > 0? mod.value.ToString():mod.value.ToString().Substring(1);
            string modValue = IsPercentageStat(mod.statsType) ? signValue + "%" : signValue;
            string mathSign = mod.value > 0? "+" : "-";
            sb.AppendLine($"{mathSign} {modValue} {modType}");
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
    
}
