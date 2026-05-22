using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Default Stat setup",fileName = "Default Stat Setup")]
public class Stat_SetUpSO : ScriptableObject
{
    [Header("Major")]
    public float Strength;
    public float Agility;
    public float Intelligence;
    public float Vitality;

    
    [Header("Rescources")]
    public float maxHealth = 100;
    public float healthRegen;

    [Header("Offense")]
    public float attackSpeed = 1;
    public float damage = 10;
    public float critChance;
    public float critPower = 150;
    public float armorReduction;
    [Space]
    public float fireDamage;
    public float iceDamage;
    public float lightningDamage;

    [Header("Defense")]
    public float armor;
    public float evasion;
    [Space]
    public float fireResistance;
    public float iceResistance;
    public float lightningResistance;

}
