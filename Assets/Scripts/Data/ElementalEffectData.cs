using System;
using UnityEngine;

[Serializable]
public class ElementalEffectData
{
    public float chillDuration;
    public float chillSlowMultiplier;

    public float burnDuration;
    public float burnDamage;

    public float shockDuration;
    public float shockDamage;
    public float shockCharge;

    public ElementalEffectData(EntityStats entityStats, DamageScaleData damageScale)
    {
        chillDuration = damageScale.chillDuration;
        chillSlowMultiplier = damageScale.chillSlowMultiplier;


        burnDuration = damageScale.burnDuration;
        burnDamage = entityStats.offenseStats.fireDamage.GetValue()* damageScale.burnDamageScale;
    
        shockDuration = damageScale.shockDuration;
        shockDamage = entityStats.offenseStats.lightningDamage.GetValue() * damageScale.shockDamageScale;
        shockCharge = damageScale.shockCharge;
    }
}
