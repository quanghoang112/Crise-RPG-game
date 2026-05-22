using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private float baseValue;
    [SerializeField] private List<StatModifier> mods = new List<StatModifier>();

    private bool isDirty=true;
    private float finalValue;
    public float GetValue()
    {
        if(isDirty)
        {
            isDirty = false;
        // Future implementation: Add modifiers from equipment, buffs, debuffs, etc.
            finalValue = GetFinalValue();
        }
        return finalValue;
    }

    public void AddModifier(float value, string source)
    {
        isDirty = true;
        mods.Add(new StatModifier(value,source));
    }

    public void RemoveModifier(string source)
    {
        isDirty = true;
        mods.RemoveAll(modifier => modifier.source == source);
    }

    private float GetFinalValue()
    {
        float finalValue = baseValue;
        
        foreach(var mod in mods)
        {
            finalValue += mod.value;
        }
        return finalValue;
    }

    public void setBaseValue(float value) => baseValue=value;
}
[Serializable]
public class StatModifier
{
    public float value;
    public string source;
    public StatModifier(float value, string source)
    {
        this.value = value;
        this.source = source;
    }

}
