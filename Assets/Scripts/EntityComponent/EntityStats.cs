// using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class EntityStats : MonoBehaviour
{
    // public Stat maxHealth;
    public Stat_SetUpSO defaultStatSetup;
    public StatResource resourceStats;
    public StatMajor majorStats;
    public StatOffense offenseStats;
    public StatDefense defenseStats;

    public float GetElementalDamage(out ElementType element, float scaleFactor = 1)
    {
        element = ElementType.None;
        float fireDamage = offenseStats.fireDamage.GetValue();
        float iceDamage = offenseStats.iceDamage.GetValue();
        float lightningDamage = offenseStats.lightningDamage.GetValue();
        float bonusElemenetalDamage = majorStats.Agility.GetValue();
        
        float highestDamage = 0;
        if(highestDamage < fireDamage)
        {
            highestDamage = fireDamage;
            element = ElementType.Fire;
        }
        else if (highestDamage < iceDamage)
        {
            highestDamage = iceDamage;
            element = ElementType.Ice;
        }
        else if (highestDamage < lightningDamage)
        {
            highestDamage = lightningDamage;
            element = ElementType.Lightning;
        }
        // float finalDamage = highestDamage + bonusElemenetalDamage;

        float bonusFire = (element == ElementType.Fire)? 0: fireDamage * .5f;
        
        float bonusIce = (element == ElementType.Ice)? 0: iceDamage * .5f;
        
        float bonusLightning = (element == ElementType.Lightning)? 0: lightningDamage * .5f;
        
        float weakerElementsDamage = bonusFire + bonusIce +bonusLightning;

        return highestDamage <= 0 ? 0 : ((highestDamage + bonusElemenetalDamage + weakerElementsDamage)*scaleFactor);
    }
    public float GetElementResistance(ElementType element)
    {
        float baseResistance = 0;
        float intelligenceResistance = majorStats.Intelligence.GetValue() * .5f;

        if(element == ElementType.Fire)
            baseResistance = defenseStats.fireResistance.GetValue();
        else if (element == ElementType.Ice)
            baseResistance = defenseStats.iceResistance.GetValue();
        else if (element == ElementType.Lightning)
            baseResistance = defenseStats.lightningResistance.GetValue();
        
        float totalResistance = (baseResistance + intelligenceResistance)/100;
        float resistanceCap = .75f;

        float finalResistance = Mathf.Clamp(totalResistance,0,resistanceCap);
        return finalResistance;
    }
    public float GetMaxHealth()
    {
        return resourceStats.maxHealth.GetValue() + majorStats.Vitality.GetValue() * 5f;
    }
    public float GetEvasion()
    {
        float baseEvasion = defenseStats.evasion.GetValue();
        float agilityEvasion = majorStats.Agility.GetValue() * 0.5f;
        float evasionCap = 85f;
        float finalEvasion = Mathf.Clamp(baseEvasion + agilityEvasion, 0f, evasionCap);
        return finalEvasion;
    }
    public float GetPhysicalDamage(out bool isCrit, float scaleFactor = 1)
    {
        float baseDamage = offenseStats.damage.GetValue();
        float strengthDamage = majorStats.Strength.GetValue() * 1f;
        float Damage = baseDamage + strengthDamage;

        float baseCritChance = offenseStats.critChance.GetValue();
        float agilityCritChance = majorStats.Agility.GetValue() * 0.3f;
        float critChance = baseCritChance + agilityCritChance;
        
        float baseCritPower = offenseStats.critPower.GetValue();
        float strengthCritPower = majorStats.Strength.GetValue() * 0.5f;
        float critPower = (baseCritPower + strengthCritPower)/100f;
        
        isCrit = Random.Range(0f,100f) < critChance;
        float finalDamage = isCrit ? Damage * (1 + critPower) : Damage;
        return finalDamage*scaleFactor;
    }

    public float GetArmorMitigation(float armorReduction)
    {
        float ScaleMitigationConstant = 100;
        float baseArmor = defenseStats.armor.GetValue();
        float vitalityArmor = majorStats.Vitality.GetValue();

        float reductionMultiplier = 1 - armorReduction;


        float totalArmor = baseArmor + vitalityArmor;

        float effectiveArmor = totalArmor * reductionMultiplier;
        float armorMitigation = effectiveArmor/(effectiveArmor + ScaleMitigationConstant);
        
        float mitigationCap = .85f;
        // Debug.Log(armorMitigation);
        return Mathf.Clamp(armorMitigation,0,mitigationCap);
    }

    public float GetArmorReduction()
    {
        float finalReduction = offenseStats.armorReduction.GetValue();
        return finalReduction;
    }
    
    public Stat GetStatByType(StatsType type)
    {
        switch(type)
        {
            case StatsType.maxHealth: return resourceStats.maxHealth;
            case StatsType.healthRegen: return resourceStats.healthRegen;
            
            case StatsType.Strength: return majorStats.Strength;
            case StatsType.Agility: return majorStats.Agility;
            case StatsType.Vitality: return majorStats.Vitality;
            case StatsType.Intelligence: return majorStats.Intelligence;

            case StatsType.attackSpeed: return offenseStats.attackSpeed;
            case StatsType.damage: return offenseStats.damage;
            case StatsType.critChance: return offenseStats.critChance;
            case StatsType.critPower: return offenseStats.critPower;
            case StatsType.armorReduction: return offenseStats.armorReduction;

            case StatsType.fireDamage: return offenseStats.fireDamage;
            case StatsType.iceDamage: return offenseStats.iceDamage;
            case StatsType.lightningDamage: return offenseStats.lightningDamage;

            case StatsType.armor: return defenseStats.armor;
            case StatsType.evasion: return defenseStats.evasion;

            case StatsType.iceResistance: return defenseStats.iceResistance;
            case StatsType.fireResistance: return defenseStats.fireResistance;
            case StatsType.lightningResistance: return defenseStats.lightningResistance;

            default:
                Debug.LogWarning($"StatsType {type} not implemented yet!");
                return null;
        }
    }

    public void ApplyDefaultStatSetup()
    {
        if(defaultStatSetup == null)
        {
            Debug.Log("No default stat set up!");
            return;
        }
        resourceStats.maxHealth.setBaseValue(defaultStatSetup.maxHealth);
        resourceStats.healthRegen.setBaseValue(defaultStatSetup.healthRegen);

        majorStats.Strength.setBaseValue(defaultStatSetup.Strength);
        majorStats.Vitality.setBaseValue(defaultStatSetup.Vitality);
        majorStats.Agility.setBaseValue(defaultStatSetup.Agility);
        majorStats.Intelligence.setBaseValue(defaultStatSetup.Intelligence);

        offenseStats.attackSpeed.setBaseValue(defaultStatSetup.attackSpeed);
        offenseStats.damage.setBaseValue(defaultStatSetup.damage);
        offenseStats.critChance.setBaseValue(defaultStatSetup.critChance);
        offenseStats.critPower.setBaseValue(defaultStatSetup.critPower);
        offenseStats.armorReduction.setBaseValue(defaultStatSetup.armorReduction);

        offenseStats.fireDamage.setBaseValue(defaultStatSetup.fireDamage);
        offenseStats.iceDamage.setBaseValue(defaultStatSetup.iceDamage);
        offenseStats.lightningDamage.setBaseValue(defaultStatSetup.lightningDamage);

        defenseStats.armor.setBaseValue(defaultStatSetup.armor);
        defenseStats.evasion.setBaseValue(defaultStatSetup.evasion);
        
        defenseStats.fireResistance.setBaseValue(defaultStatSetup.fireResistance);
        defenseStats.iceResistance.setBaseValue(defaultStatSetup.iceResistance);
        defenseStats.lightningResistance.setBaseValue(defaultStatSetup.lightningResistance);
        
    }
}
