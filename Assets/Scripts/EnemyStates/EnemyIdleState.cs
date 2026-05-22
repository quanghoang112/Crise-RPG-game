using UnityEngine;

public class EnemyIdleState : EnemyGroundedState
{
    public EnemyIdleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.setVelocity(0f, enemy.rb.linearVelocity.y);
        stateTimer = enemy.moveTimerDuration;
    }
    public override void Update()
    {
        base.Update();
        if(stateTimer <= 0f)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }
}
