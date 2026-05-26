using System.Threading.Tasks;
using UnityEngine;

public class SkillObject_Base : MonoBehaviour
{
    [SerializeField] protected LayerMask whatIsEnemy;
    protected Transform targetCheck;
    [SerializeField] protected float checkRadius = 1;

    protected EntityStats playerStats;
    protected DamageScaleData damageScaleData;

    protected void DamageEnemiesInRadius(Transform t, float radius)
    {
        foreach(var target in EnemiesAround(t,radius))
        {
            IDamagable damagable = target.GetComponent<IDamagable>();
            
            if(damagable == null)
                return;

            EntityStatusHandler statusHandler = target.GetComponent<EntityStatusHandler>();

            AttackData attackData = playerStats.GetAttackData(damageScaleData);

            ElementalEffectData effectData = new ElementalEffectData(playerStats, damageScaleData);

            float physicalDamage = attackData.physicalDamage;
            float elementalDamage = attackData.elementalDamage;
            ElementType element = attackData.element;

            damagable.TakeDamage(1,1,element,transform);
        
            if(element != ElementType.None)
                statusHandler.ApplyStatusEffect(element, effectData);
        }
    }

    protected Transform FindClosestTarget()
    {
        Transform target = null;
        float closestDistance = Mathf.Infinity;

        foreach(var enemy in EnemiesAround(transform,checkRadius))
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if(distance < closestDistance)
            {
                target = enemy.transform;
                closestDistance = distance;
            }
        }
        return target;
    }

    protected Collider2D[] EnemiesAround(Transform t, float radius)
    {
        return Physics2D.OverlapCircleAll(t.position, radius, whatIsEnemy);
    }

    protected virtual void OnDrawGizmos()
    {
        if(targetCheck == null)
        {
            targetCheck = transform;
        }

        Gizmos.DrawWireSphere(targetCheck.position,checkRadius);
    }
}
