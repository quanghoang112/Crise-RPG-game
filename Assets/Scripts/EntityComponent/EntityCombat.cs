using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class EntityCombat : MonoBehaviour
{
    public Collider2D[] targetColliders;
    public EntityVFX entityVFX;
    public EntityStats entityStats;
    public float damage;
    public float elementalDamage;
    public bool isCrit;

    [Header("Target detection")]
    public Transform targetCheck;
    public float targetCheckRadius;
    public LayerMask whatIsTarget;

    [Header("Status effect details")]
    [SerializeField] private float defaultDuration = 3;
    [SerializeField] private float chillSlowMultiplier = .2f;
    [SerializeField] private float electrifyChargeBuildUp = 0.4f;

    public void Awake()
    {
        entityVFX = GetComponent<EntityVFX>();
        entityStats = GetComponent<EntityStats>();
        damage = entityStats.GetPhysicalDamage(out isCrit);
    }

    public void Update()
    {
        damage = entityStats.GetPhysicalDamage(out isCrit);
    }
    public void performAttack()
    {
        handleTargetDetection();
        damage = entityStats.GetPhysicalDamage(out isCrit);
        elementalDamage = entityStats.GetElementalDamage(out ElementType element);   

        foreach(var targetCollider in targetColliders)
        {
            IDamagable targetHealth = targetCollider.GetComponent<IDamagable>();
            if(targetHealth == null)
                continue;
            if(targetHealth.TakeDamage(damage,elementalDamage,element,transform))
            {
                applyStatusEffect(targetCollider.transform,element);
                entityVFX.updateOnHitVFXColor(element);
                if(!isCrit)
                    entityVFX.CreateOnHitVFX(targetCollider.transform);
                else
                    entityVFX.CreateOnCritHitVFX(targetCollider.transform);
            }
        }
    }

    public void applyStatusEffect (Transform target, ElementType element)
    {
        EntityStatusHandler statusHandler = target.GetComponent<EntityStatusHandler>();
        if(!statusHandler)  return;
        if(statusHandler.CanBeApllied(element))
        if(element == ElementType.Fire)
        {
            Debug.Log("Fire");
            statusHandler.ApplyBurnEffect(defaultDuration,elementalDamage);
        }
        else if (element == ElementType.Ice)
        {
            Debug.Log("Ice");
            statusHandler.ApplyChilledEffect(defaultDuration,chillSlowMultiplier);
        }
        else if (element == ElementType.Lightning)
        {
            Debug.Log("Lighting");
            statusHandler.ApplyElectrifyEffect(defaultDuration,elementalDamage*3,electrifyChargeBuildUp);
        }
    }

    protected Collider2D[] handleTargetDetection()
    {
        targetColliders = Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
        return targetColliders;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
