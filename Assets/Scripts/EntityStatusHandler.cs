using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class EntityStatusHandler : MonoBehaviour
{
    private ElementType currEffect = ElementType.None;
    private Entity entity;
    private EntityVFX entityVFX;
    private EntityStats entityStats;
    private EntityHealth entityHealth;

    [Header("Electrify effect details")]
    [SerializeField] private GameObject lightningVFX;
    [SerializeField] private float currCharge;
    [SerializeField] private float maximumCharge = 1;


    private Coroutine chilledEffectCo;
    private Coroutine burnEffectCo;
    private Coroutine electrifyCo;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        entityVFX = GetComponent<EntityVFX>();
        entityStats = GetComponent<EntityStats>();
        entityHealth = GetComponent<EntityHealth>();
    }

    public void ApplyStatusEffect(ElementType element, ElementalEffectData effectData)
    {
        if(element == ElementType.Ice && CanBeApllied(ElementType.Ice))
        {
            ApplyChilledEffect(effectData.chillDuration,effectData.chillSlowMultiplier);
        }
        if(element == ElementType.Fire && CanBeApllied(ElementType.Fire))
        {
            ApplyBurnEffect(effectData.burnDuration,effectData.burnDamage);
        }
        if(element == ElementType.Lightning && CanBeApllied(ElementType.Lightning))
        {
            ApplyElectrifyEffect(effectData.shockDuration,effectData.shockDamage,effectData.shockCharge);
        }
    }

    public void ApplyChilledEffect (float duration, float slowMultiplier)
    {
        Debug.Log("asdasd");
        float iceResistance = entityStats.GetElementResistance(ElementType.Ice);
        float reducedDuration = duration * ( 1 - iceResistance);

        if(chilledEffectCo != null)
            StopCoroutine(chilledEffectCo);
        chilledEffectCo = StartCoroutine(ChilledEffectCo(reducedDuration,slowMultiplier));
    }

    public void ApplyBurnEffect(float duration, float totalDamage)
    {
        float fireResistance = entityStats.GetElementResistance(ElementType.Fire);
        float reducedDuration = duration * ( 1 - fireResistance);
        if(burnEffectCo != null)
            StopCoroutine(burnEffectCo);
        burnEffectCo = StartCoroutine(BurnEffectCo(reducedDuration,totalDamage));
    }

    public void ApplyElectrifyEffect(float duration, float damage, float charge)
    {
        float lightningResistance = entityStats.GetElementResistance(ElementType.Lightning);
        float finalCharge = charge * (1 - lightningResistance);
        currCharge = currCharge + finalCharge;
        if(currCharge > maximumCharge)
        {
            Instantiate(lightningVFX, transform.position, quaternion.identity);
            entityHealth.reduceHp(damage);
            StopElectrifyEffect();
            return;
        }
        if(electrifyCo != null)
        {
            StopCoroutine(electrifyCo);
        }
        electrifyCo=StartCoroutine(ElectrifyEffectCo(duration));
    }



    // private IEnumerator 
    private IEnumerator BurnEffectCo(float duration, float totalDamage)
    {
        currEffect = ElementType.Fire;
        
        entityVFX.PlayStatusVfx(duration,currEffect);

        int ticksPerSecond = 2;
        int tickCount = Mathf.RoundToInt(ticksPerSecond * duration);

        float damagePerTick = totalDamage / tickCount;
        float tickInterval = 1f/ticksPerSecond;

        for (int i = 0; i< tickCount; i++)
        {
            entityHealth.reduceHp(damagePerTick);

            yield return new WaitForSeconds(tickInterval);
        }

        currEffect = ElementType.None;
    }
    private IEnumerator ChilledEffectCo (float duration, float slowMultiplier)
    {
        currEffect = ElementType.Ice;
        
        entity.slowDownEntityBy(duration,slowMultiplier);
        entityVFX.PlayStatusVfx(duration,currEffect);

        yield return new WaitForSeconds(duration);

        currEffect = ElementType.None;
    }
    private IEnumerator ElectrifyEffectCo(float duration)
    {
        currEffect = ElementType.Lightning;
        entityVFX.PlayStatusVfx(duration,currEffect);

        yield return new WaitForSeconds(duration);

        StopElectrifyEffect();
    }
    public bool CanBeApllied (ElementType element)
    {
        if(element == ElementType.Lightning && currEffect == ElementType.Lightning)
            return true;
        return currEffect == ElementType.None;
    }
    private void StopElectrifyEffect()
    {
        currEffect = ElementType.None;
        currCharge = 0;
        entityVFX.StopAllVFX();
    }
    
}
