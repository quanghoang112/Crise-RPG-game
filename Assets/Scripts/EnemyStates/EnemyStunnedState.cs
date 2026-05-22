using Unity.VisualScripting;
using UnityEngine;

public class EnemyStunnedState : EnemyState
{
    private EnemyAnimTrigger enemyAnimTrigger;
    public EnemyStunnedState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        enemyAnimTrigger = enemy.GetComponentInChildren<EnemyAnimTrigger>();
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enemy stunned");
        enemyAnimTrigger.disableCounterWindow();
        stateTimer = enemy.stunnedDuration;
        rb.linearVelocity = new Vector2(enemy.stunnedVelocity.x * -enemy.facingDir, enemy.stunnedVelocity.y);
    }

    public override void Update()
    {
        base.Update();
        if(stateTimer <= 0)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
