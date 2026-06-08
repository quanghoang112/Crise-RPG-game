using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    public SkillDash dash {get;private set;}

    public SkillShard shard {get;private set;}
    public SkillThrowSword throwSword {get; private set;}
    public SkillTimeEcho timeEcho {get;private set;}
    public SkillDomain domainExpansion {get;private set;}
    public SkillBase[] allSkills{get; private set;}
    
    
    private void Awake()
    {
        dash = GetComponentInChildren<SkillDash>();
        shard = GetComponentInChildren<SkillShard>();
        throwSword = GetComponentInChildren<SkillThrowSword>();
        timeEcho = GetComponentInChildren<SkillTimeEcho>();
        domainExpansion = GetComponentInChildren<SkillDomain>();
        allSkills = GetComponentsInChildren<SkillBase>();
    }

    public void reduceAllSKillCooldownBy(float amount)
    {
        foreach(var skill in allSkills)
        {
            skill.ReduceCooldownBy(amount);
        }
    }


    public SkillBase GetSkillByType(SkillType type)
    {
        switch(type)
        {
            case SkillType.Dash: return dash;
            case SkillType.TimeShard: return shard;
            case SkillType.SwordThrow: return throwSword;
            case SkillType.TimeEcho: return timeEcho;
            case SkillType.DomainExpansion: return domainExpansion;

            default:
                return null;
        }
    }
}
