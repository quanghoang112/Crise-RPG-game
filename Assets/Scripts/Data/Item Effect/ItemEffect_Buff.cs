using System;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Buff Effect", fileName = "Item effect data - Buff")]
public class ItemEffect_Buff : ItemEffectDataSO
{
    [SerializeField] private BuffEffectData[] buffsToApply;
    [SerializeField] private float duration;
    [SerializeField] private string source = Guid.NewGuid().ToString();

    // private PlayerStats playerStats;

    public override bool CanBeUsed(Player player)
    {
        
        if(player.entityStats.CanApplyBufffOf(source))
        {
            this.player = player;
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
        player.entityStats.ApplyBuff(buffsToApply,duration,source);
    }

}
