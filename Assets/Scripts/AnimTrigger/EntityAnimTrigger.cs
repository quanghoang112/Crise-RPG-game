using UnityEngine;

public class EntityAnimTrigger : MonoBehaviour
{
    private Entity entity;
    private EntityCombat entityCombat;

    protected virtual void Awake()
    {
        entity = GetComponentInParent<Entity>();
        entityCombat = GetComponentInParent<EntityCombat>();
    }

    public void currentStateTrigger()
    {
        entity.callAnimationTrigger();
    }
    private void attackTrigger()
    {
        Debug.Log("Attack Triggered");
        entityCombat.performAttack();
    }
}
