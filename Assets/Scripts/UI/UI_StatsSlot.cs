using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class UI_StatsSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private EntityStats playerStats;
    private RectTransform rect;
    private UI ui;

    [SerializeField] private StatsType statsType;
    [SerializeField] private TextMeshProUGUI statName;
    [SerializeField] private TextMeshProUGUI statValue;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        playerStats = FindAnyObjectByType<PlayerStats>();
    
    }

    private void OnValidate()
    {
        gameObject.name = "UI_Stat - " + GetStatNameByType(statsType);
        statName.text = GetStatNameByType(statsType);
        // UpdateStatValue();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(ui.statsToolTip == null) return;
        ui.statsToolTip.showToolTip(true,rect,statsType);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statsToolTip.showToolTip(false, null);
    }


    public void UpdateStatValue()
    {
        Stat statToUpdate = playerStats.GetStatByType(statsType);

        if(statToUpdate == null && statsType != StatsType.ElementalDamage)
            return;
        
        float value = 0;

        switch(statsType)
        {
            case StatsType.Strength:
                value = playerStats.majorStats.Strength.GetValue();
                break;
            case StatsType.Agility:
                value = playerStats.majorStats.Agility.GetValue();
                break;
            case StatsType.Intelligence:
                value = playerStats.majorStats.Intelligence.GetValue();
                break;
            case StatsType.Vitality:
                value = playerStats.majorStats.Vitality.GetValue();
                break;
            
            case StatsType.damage:
                value = playerStats.GetBaseDamage();
                break;
            case StatsType.critChance:
                value = playerStats.GetCritChance();
                break;
            case StatsType.critPower:
                value = playerStats.GetCritPower();
                break;
            case StatsType.armorReduction:
                value = playerStats.GetArmorReduction() * 100;
                break;
            case StatsType.attackSpeed:
                value = playerStats.offenseStats.attackSpeed.GetValue() * 100;
                break;
            
            case StatsType.maxHealth:
                value = playerStats.GetMaxHealth();
                break;
            case StatsType.healthRegen:
                value = playerStats.resourceStats.healthRegen.GetValue();
                break;
            case StatsType.evasion:
                value = playerStats.GetEvasion();
                break;
            case StatsType.armor:
                value = playerStats.GetBaseArmor();
                break;
            
            case StatsType.iceDamage:
                value = playerStats.offenseStats.iceDamage.GetValue();
                break;
            case StatsType.lightningDamage:
                value = playerStats.offenseStats.lightningDamage.GetValue();
                break;
            case StatsType.fireDamage:
                value = playerStats.offenseStats.fireDamage.GetValue();
                break;
            case StatsType.ElementalDamage:
                value =  playerStats.GetElementalDamage(out ElementType element, 1);
                break;

            case StatsType.iceResistance:
                value = playerStats.defenseStats.iceResistance.GetValue() *100;
                break;
            case StatsType.fireResistance:
                value = playerStats.defenseStats.fireResistance.GetValue() *100;
                break;
            case StatsType.lightningResistance:
                value = playerStats.defenseStats.lightningResistance.GetValue() *100;
                break;
        }

        statValue.text = IsPercentageStat(statsType) ? value + "%" : value.ToString();
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
            case StatsType.ElementalDamage: return "Elemental Damage";

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
}
