using UnityEngine;

public class SkillObject_AnimTrigger : MonoBehaviour
{
    private SkillObject_TimeEcho timeEcho;

    private void Awake()
    {
        timeEcho = GetComponentInParent<SkillObject_TimeEcho>();
    }

    private void attackTrigger()
    {
        timeEcho.PerformAttack();
    }

    private void TryTerminate(int currentAttackIndex)
    {
        if(currentAttackIndex == timeEcho.maxAttacks)
            timeEcho.HandleDeath();
    }
}
