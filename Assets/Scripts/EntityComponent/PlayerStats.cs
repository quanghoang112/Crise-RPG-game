using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public class PlayerStats : EntityStats
{

    private List<string> activeBuff = new List<string>();
    private InventoryBase inventory;

    protected override void Awake()
    {
        base.Awake();

        inventory = GetComponent<InventoryBase>();
    }

    public bool CanApplyBufffOf(string source)
    {
        return activeBuff.Contains(source) == false;
    }

    public void ApplyBuff (BuffEffectData[] buffsToApply, float duration, string source)
    {
        //StartCoroutine()
        StartCoroutine(BuffCo(buffsToApply,duration,source));
    }


    private IEnumerator BuffCo(BuffEffectData[] buffsToApply, float duration, string source)
    {
        activeBuff.Add(source);

        foreach (var buff in buffsToApply)
        {
            GetStatByType(buff.type).AddModifier(buff.value,source);
        }

        yield return new WaitForSeconds(duration);

        foreach(var buff in buffsToApply)
        {
            GetStatByType(buff.type).RemoveModifier(source);
        }
        activeBuff.Remove(source);

        inventory.OnTriggerUpdateUI();
    }
}
