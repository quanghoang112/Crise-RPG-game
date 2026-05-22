using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    public SkillDash dash {get;private set;}

    private void Awake()
    {
        dash = GetComponentInChildren<SkillDash>();
    }


    public SkillBase GetSkillByType(SkillType type)
    {
        switch(type)
        {
            case SkillType.Dash: return dash;

            default:
                return null;
        }
    }
}
