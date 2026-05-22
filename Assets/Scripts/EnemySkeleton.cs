using UnityEngine;

public class EnemySkeleton : Enemy, ICounterable
{
    public bool CanBeCountered { get => canBeStunned; set => canBeStunned = value; }
    protected override void Awake()
    {
        base.Awake();
        idleState = new EnemyIdleState(this,stateMachine, "Idle");
        moveState = new EnemyMoveState(this,stateMachine, "Move");
        attackState = new EnemyAttackState(this,stateMachine, "Attack");
        battleState = new EnemyBattleState(this,stateMachine, "Battle");
        deathState = new EnemyDeathState(this,stateMachine, "Death");
        stunnedState = new EnemyStunnedState(this,stateMachine, "Stunned");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    public void HandleCounter()
    {
        if(!CanBeCountered) return;
        // Debug.Log("Countered");
        stateMachine.ChangeState(stunnedState);
    }
}
