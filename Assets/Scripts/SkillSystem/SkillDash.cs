using UnityEngine;

public class SkillDash : SkillBase
{
    // private SkillShard shard;

    private void Awake()
    {
        skillType = SkillType.Dash;
    }
    public void OnStartEffect()
    {
        if(Unlocked(SkillUpgradeType.DashCloneOnStart) || Unlocked(SkillUpgradeType.DashCloneOnStartAndArrival))
        {
            CreateClone();
        }
        if(Unlocked(SkillUpgradeType.DashShardOnStart) || Unlocked(SkillUpgradeType.DashShardStartAndArrival))
        {
            CreateShard();
        }
    }
    
    public void OnEndEffect()
    {
        if(Unlocked(SkillUpgradeType.DashCloneOnStartAndArrival))
        {
            CreateClone();
        }
        if(Unlocked(SkillUpgradeType.DashShardStartAndArrival))
        {
            CreateShard();
        }
    }

    private void CreateShard()
    {
        Debug.Log("Create time shard!");
    }

    private void CreateClone()
    {
        Debug.Log("Create time echo!");
    }
}
