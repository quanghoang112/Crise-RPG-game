using UnityEngine;
using UnityEngine.UI;

public class EnemyDeathState : EnemyState
{
    private Collider2D col=> enemy.GetComponent<Collider2D>();
    private EnemyVFX enemyVFX;
    private Slider healthBar;
    // private Image healthIcon;
    // private Canvas healthCanvas;
    public EnemyDeathState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        // col = enemy.GetComponent<Collider2D>();
        enemyVFX = enemy.GetComponentInChildren<EnemyVFX>();
        healthBar = enemy.GetComponentInChildren<Slider>();
        // healthIcon = enemy.GetComponentInChildren<Image>();
        // healthCanvas = enemy.GetComponentInChildren<Canvas>();
    }

    public override void Enter()
    {
        base.Enter();
        // col.enabled = false;
        // rb.gravityScale = 12;
        // enemy.setVelocity(rb.linearVelocity.x, 20f);
        enemyVFX.EnableAttackAlert(false);
        enemy.transform.Find("HealthBarUI").gameObject.SetActive(false);
        stateMachine.swithOffStateMachine();
    }
    public override void Update()
    {
        base.Update();
        if(triggerCalled)
        {
            anim.enabled = false;
        }
    }
}
