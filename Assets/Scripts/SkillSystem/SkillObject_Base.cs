using System.Threading.Tasks;
using UnityEngine;

public class SkillObject_Base : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;
    protected EntityStats playerStats;
    protected DamageScaleData damageScaleData;


    [SerializeField] private GameObject onHitVfx;

    [Space]
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected Transform targetCheck;
    [SerializeField] protected float checkRadius = 1;
    protected Transform lastTarget;

    protected bool targetGotHit;

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    protected void DamageEnemiesInRadius(Transform t, float radius)
    {
        foreach(var target in GetEnemiesAround(t,radius))
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

            targetGotHit = damagable.TakeDamage(physicalDamage,elementalDamage,element,transform);

            if(targetGotHit)
            {
                lastTarget = target.transform;
                Instantiate(onHitVfx,target.transform.position,Quaternion.identity);
            }
            if(element != ElementType.None)
                statusHandler.ApplyStatusEffect(element, effectData);
        }
    }

    protected Transform FindClosestTarget()
    {
        Transform target = null;
        float closestDistance = Mathf.Infinity;

        foreach(var enemy in GetEnemiesAround(transform,checkRadius))
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

    protected Collider2D[] GetEnemiesAround(Transform t, float radius)
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
