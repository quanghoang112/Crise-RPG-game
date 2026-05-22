using UnityEngine;

public class EnemyAnimTrigger : EntityAnimTrigger
{
    private Enemy enemy => GetComponentInParent<Enemy>();
    private EnemyVFX enemyVFX => GetComponentInParent<EnemyVFX>();

    public void enableCounterWindow()
    {
        enemy.enableCounterWindow(true);
        enemyVFX.EnableAttackAlert(true);
    }
    public void disableCounterWindow()
    {
        enemy.enableCounterWindow(false);
        enemyVFX.EnableAttackAlert(false);
    }
}
