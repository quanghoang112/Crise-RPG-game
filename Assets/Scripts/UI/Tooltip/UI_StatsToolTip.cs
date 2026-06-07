using TMPro;
using UnityEngine;

public class UI_StatsToolTip : UI_ToolTip
{
    private PlayerStats playerStats;
    [SerializeField] private TextMeshProUGUI statToolTipText;

    protected override void Awake()
    {
        base.Awake();

        playerStats = FindAnyObjectByType<PlayerStats>();
    }

    public void showToolTip(bool show, RectTransform targetRect, StatsType statsType)
    {
        base.showToolTip(show, targetRect);
        statToolTipText.text = GetStatTextByType(statsType);
    }

    public string GetStatTextByType(StatsType type)
    {
        switch (type)
        {
            // Major Attributes
            case StatsType.Strength:
                return "Increases physical damage by 1 per point." +
                       "\n Increases critical power by 0.5% per point.";
            case StatsType.Agility:
                return "Increases critical chance by 0.3% per point." +
                       "\n Increases evasion by 0.5% per point.";
            case StatsType.Intelligence:
                return "Increases elemental resistances by 0.5% per point." +
                        "\n Adds 1 elemental damage per point as a bonus. " +
                        "\n If all elements have 0 damage, the bonus will not be applied.";
            case StatsType.Vitality:
                return "Increases maximum health by 5 per point" +
                       "\n Increases armor by 1 per point.";

            // Physical Damage
            case StatsType.damage:
                return "Determines the physical damage of your attacks.";
            case StatsType.critChance:
                return "Chance for your attacks to critically strike.";
            case StatsType.critPower:
                return "Increases the damage dealt by critical strikes.";
            case StatsType.armorReduction:
                return "Percent of armor that will be ignored by your attacks.";
            case StatsType.attackSpeed:
                return "Determines how quickly you can attack.";

            // Defense
            case StatsType.maxHealth:
                return "Determines how much total health you have.";
            case StatsType.healthRegen:
                return "Amount of health restored per second.";
            case StatsType.armor:
                return "Reduces incoming physical damage."
                    + "\n Armor mitigation is Limited at 85%."
                    + "Current mitigation is: " + playerStats.GetArmorMitigation(0) * 100 + "%.";
            case StatsType.evasion:
                return "Chance to completely avoid attacks." + "\n Limited at 85%.";

            // Elemental Damage
            case StatsType.iceDamage:
                return "Determines the ice damage of your attacks.";
            case StatsType.fireDamage:
                return "Determines the fire damage of your attacks.";
            case StatsType.lightningDamage:
                return "Determines the lightning damage of your attacks.";
            case StatsType.ElementalDamage:
                return
                    "Elemental damage combines all three elements. " +
                    "\n The highest element applies corresponding element status effect and full damage. " +
                    "\n The other two elements contribute 50% of their damage as a bonus.";

            // Elemental Resistances
            case StatsType.iceResistance:
                return "Reduces ice damage taken.";
            case StatsType.fireResistance:
                return "Reduces fire damage taken.";
            case StatsType.lightningResistance:
                return "Reduces lightning damage taken.";

            default:
                return "No tooltip avalible for this stat.";
        }
    }
}
