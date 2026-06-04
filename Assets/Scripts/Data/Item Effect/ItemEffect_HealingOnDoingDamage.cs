using UnityEngine;


[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Heal on doing damage", fileName = "Item effect data - heal on doing physical damage")]
public class ItemEffect_HealingOnDoingDamage : ItemEffectDataSO
{
    [SerializeField] private float percentHealedOnAttack = .2f;




    public override void ExecuteEffect()
    {
        base.ExecuteEffect();
    }

    public override void Subscribe(Player player)
    {
        base.Subscribe(player);

        player.playerCombat.OnDoingPhysicalDamage += HealOnDoingDamage;
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();

        player.playerCombat.OnDoingPhysicalDamage-=HealOnDoingDamage;
        player = null;
    }

    private void HealOnDoingDamage(float damage)
    {
        player.entityHealth.IncreaseHealth(damage * percentHealedOnAttack);
    }
}
