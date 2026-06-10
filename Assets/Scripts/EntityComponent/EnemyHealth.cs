using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class EnemyHealth : EntityHealth
{
    // private Player player;
    private Enemy enemy => GetComponent<Enemy>();
    public override bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        // player = damageDealer.GetComponent<Player>();
        if(!canTakeDamage)
            return false;
        
        bool wasHit = base.TakeDamage(damage,elementalDamage,element, damageDealer);

        if(!wasHit) return false;
        if(damageDealer.GetComponent<Player>() != null && !isDead)
        {
            enemy.tryEnterBattleState(damageDealer);
        }
        return true;
    }
    protected override void Die()
    {
        base.Die();
        Player.instance.inventory.gold += Random.Range(1000,10000);
        enemy.dropManager?.DropItems();
        // enemy.EntityDeath();
    }
}
