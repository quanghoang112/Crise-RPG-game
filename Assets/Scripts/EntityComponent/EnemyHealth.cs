using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class EnemyHealth : EntityHealth
{
    private Enemy enemy => GetComponent<Enemy>();
    public override bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
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
        // enemy.EntityDeath();
    }
}
