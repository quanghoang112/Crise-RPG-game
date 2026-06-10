using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EntityHealth : MonoBehaviour, IDamagable
{
    public event Action OnTakingDamage;
    public event Action OnhealthUpdate;

    private Slider healthBar;
    private EntityVFX entityVFX => GetComponent<EntityVFX>();
    private Entity entity => GetComponent<Entity>();
    private EntityStats entityStats => GetComponent<EntityStats>();
    private EntityDropManager dropManager => GetComponent<EntityDropManager>();


    private bool miniHealthBarActive;
    [SerializeField] protected float maxHp;
    [SerializeField] protected bool isDead;
    [SerializeField] public float currentHp;
    protected bool canTakeDamage = true;
    public float lastDamageTaken{get; private set;}

    [Header("regen health")]
    [SerializeField] private float regenInterval = 1;
    [SerializeField] private bool canRegen = true;

    [Header("On Damage Knockback")]
    [SerializeField] protected Vector2 knockbackPower = new Vector2(1.5f,2.5f);
    [SerializeField] protected Vector2 heavyDamageKnockbackPower = new Vector2(3f,4f);
    [SerializeField] protected float knockbackDuration = 0.25f;
    [SerializeField] protected float heavyKnockbackDuration = 0.35f;

    [Header("On Heavy Damage")]
    [SerializeField] private float heavyDamageThreshold = 0.3f; // Percentage of max HP

    public virtual void Awake()
    {
        if(entityStats != null)
        {
            maxHp = entityStats.GetMaxHealth();
            currentHp = maxHp;
            healthBar = GetComponentInChildren<Slider>();
            OnhealthUpdate += updateHealthBar;
            
            updateHealthBar();
            InvokeRepeating(nameof(RegenHealth),0, regenInterval);
        }
        canTakeDamage = true;
    }

    public virtual void Update()
    {
        if(entityStats == null) return;
        maxHp = entityStats.GetMaxHealth();
    }
    public virtual bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        if(isDead || !canTakeDamage)
        {
            return false;
        }
        if(AttackEvaded())
        {
            return false;
        }

        EntityStats attackerStats = damageDealer.GetComponent<EntityStats>();

        float armorReduction = attackerStats!= null ? attackerStats.GetArmorReduction() : 0;
        float resistance = entityStats != null ? entityStats.GetElementResistance(element) : 0;
        float mitigation = entityStats != null ? entityStats.GetArmorMitigation(armorReduction) : 0;

        float takenDamage = damage*(1- mitigation);
        float takenElementalDamage = elementalDamage*(1-resistance);

        takeKnockback(takenDamage,damageDealer);

        reduceHp(takenDamage + takenElementalDamage);

        lastDamageTaken = takenDamage + takenElementalDamage;

        OnTakingDamage?.Invoke();

        Debug.Log("Damage:" + damage + ", takenDamage: " + takenDamage + ", elementalDamage: " + elementalDamage + ", takenElemental: " + takenElementalDamage + ", element: " + element);
        return true;
    }

    public void SetCanTakeDamage(bool CanTakeDamage) => this.canTakeDamage = CanTakeDamage;

    public void takeKnockback(float takenDamage, Transform damageDealer)
    {
        int knockbackDir = calculateKnockbackDirection(damageDealer);
        entity?.receiveKnockback(isHeavyDamage(takenDamage) ? 
        new Vector2(heavyDamageKnockbackPower.x * knockbackDir,heavyDamageKnockbackPower.y) :
        new Vector2(knockbackPower.x * knockbackDir,knockbackPower.y), isHeavyDamage(takenDamage) ? heavyKnockbackDuration : knockbackDuration);
    }

    private bool AttackEvaded()
    {
        if(entityStats == null) return false;
        float evasionChance = entityStats.GetEvasion();
        return UnityEngine.Random.Range(0, 100) < evasionChance;
    }

    public void IncreaseHealth (float healAmount)
    {
        if(isDead)  return;

        float newHealth = currentHp + healAmount;
        float maxHealth = entityStats.GetMaxHealth();

        if(newHealth > maxHealth)
            currentHp = maxHealth;
        else
            currentHp = newHealth;
        
        // updateHealthBar();
        OnhealthUpdate?.Invoke();
    }

    private void RegenHealth()
    {
        if(!canRegen)   return;
        
        float regenAmount = entityStats.resourceStats.healthRegen.GetValue();
        IncreaseHealth(regenAmount);
    }

    public void reduceHp(float damage)
    {
        
        currentHp -= damage;
        
        entityVFX?.PlayOnDamageVFX();
        // updateHealthBar();
        OnhealthUpdate?.Invoke();
        
        if(currentHp <= 0)
        {
            currentHp = 0;
            Die();
        }
    }
    
    protected virtual void Die()
    {
        isDead = true;
        entity?.EntityDeath();
        // Debug.Log("Entity died");
        dropManager?.DropItems();
    }

    public float GetHealthPercent() => currentHp / entityStats.GetMaxHealth();
    public float GetCurrentHealth() => currentHp;

    public void SetHealthToPercent(float percent)
    {
        currentHp = entityStats.GetMaxHealth() * Mathf.Clamp01(percent);
        // updateHealthBar();
        OnhealthUpdate?.Invoke();
    }
    private void updateHealthBar()
    {
        if(healthBar == null && healthBar.transform.parent.gameObject.activeSelf == false)
            return;

        healthBar.value = currentHp / entityStats.GetMaxHealth(); 
    }

    public void EnableHealthBar(bool enable) => healthBar?.transform.parent.gameObject.SetActive(enable);
    private int calculateKnockbackDirection(Transform damageDealer)
    {
        int knockbackDir = transform.position.x - damageDealer.position.x > 0 ? 1 : -1;
        return knockbackDir;
    }
    private bool isHeavyDamage(float damage)
    {
        if(entityStats == null) return false;
        return damage >= maxHp * heavyDamageThreshold;
    }
}
