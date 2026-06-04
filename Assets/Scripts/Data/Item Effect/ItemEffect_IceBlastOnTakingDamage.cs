using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Ice blast", fileName = "Item effect data - Ice blast on taking damage")]
public class ItemEffect_IceBlastOnTakingDamage : ItemEffectDataSO
{
    [SerializeField] private ElementalEffectData effectData;
    [SerializeField] private float iceDamage;
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private float checkRadius;
    
    [Space]
    [SerializeField] private float healthPercentTrigger = .25f;
    [SerializeField] private float cooldown;
    [SerializeField] private float lastTimeUsed = -999;
    [Header("Vfx Objects")]
    [SerializeField] private GameObject iceBlastVfx;
    [SerializeField] private GameObject onHitVfx;

    private void OnEnable()
    {
        lastTimeUsed = -999f;
    }


    public override void ExecuteEffect()
    {
        // Debug.Log(lastTimeUsed);
        bool noCooldown = Time.time >= lastTimeUsed + cooldown;
        bool reachedThreshold = player.entityHealth.GetHealthPercent() <= healthPercentTrigger;
        // Debug.Log($"{noCooldown} {reachedThreshold}");
        if(noCooldown && reachedThreshold)
        {
            Debug.Log("Ice blast");
            player.vfx.CreateEffectOf(iceBlastVfx,player.transform);
            lastTimeUsed = Time.time;
            DamageEnemiesWithIce();
        }
    
    }

    private void DamageEnemiesWithIce()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(player.transform.position,checkRadius,whatIsEnemy);

        foreach(var enemy in enemies)
        {
            IDamagable damagable = enemy.GetComponent<IDamagable>();

            if(damagable == null)   return;
            bool targetGotHit = damagable.TakeDamage(0, iceDamage, ElementType.Ice, player.transform);
            EntityStatusHandler statusHandler = enemy.GetComponent<EntityStatusHandler>();
            statusHandler?.ApplyStatusEffect(ElementType.Ice,effectData);

            if(targetGotHit)
                player.vfx.CreateEffectOf(onHitVfx,enemy.transform);
        }
    }

    public override void Subscribe(Player player)
    {
        base.Subscribe(player);
        player.entityHealth.OnTakingDamage += ExecuteEffect;
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();

        player.entityHealth.OnTakingDamage -= ExecuteEffect;
        player = null;
    }
}
