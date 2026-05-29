using UnityEngine;

public class SkillObject_Health : EntityHealth
{
    protected override void Die()
    {
        // base.Die();

        SkillObject_TimeEcho timeEcho = GetComponent<SkillObject_TimeEcho>();
        timeEcho.HandleDeath();
    }
}
