using System;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Buff Effect", fileName = "Item effect data - Buff")]
public class ItemEffect_Buff : ItemEffectDataSO
{
    [SerializeField] private BuffEffectData[] buffsToApply;
    [SerializeField] private float duration;
    [SerializeField] private string source = Guid.NewGuid().ToString();

    private PlayerStats playerStats;

    public override bool CanBeUsed()
    {
        if(playerStats == null)
            playerStats = FindAnyObjectByType<PlayerStats>();

        if(playerStats.CanApplyBufffOf(source))
        {
            Debug.Log("Used it!");
            return true;
        }
        else
        {
            Debug.Log("Same buff!");
            return false;
        }
    }
    public override void ExecuteEffect()
    {
        playerStats.ApplyBuff(buffsToApply,duration,source);
    }

}
