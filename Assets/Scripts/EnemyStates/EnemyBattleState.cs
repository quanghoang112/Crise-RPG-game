using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyBattleState : EnemyState
{
    private Transform player;
    private float lastTimeWasInBattle;
    public EnemyBattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        updateBattleTimer();

        if(player == null)
        {
            player = enemy.GetPlayerReference();
        }
        if(shouldRetreat())
        {
            Debug.Log("Retreating");
            rb.linearVelocity = new Vector2(enemy.retreatVelocity.x * -directionToPlayer(),enemy.retreatVelocity.y);
            enemy.handleFlip(directionToPlayer());
            stateTimer = 0.5f;
        }
    }

    public override void Update()
    {
        base.Update();
        if(stateTimer >0f)
        {
            return;
        }
        if(withInAttackRange())
        {
            stateMachine.ChangeState(enemy.attackState);
        }
        else
        {
            int direction = directionToPlayer();
            enemy.setVelocity(enemy.battleMoveSpeed * direction,enemy.rb.linearVelocity.y);
        }
        if(battleTimeIsOver())
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
    private void updateBattleTimer() => lastTimeWasInBattle = Time.time;
    private bool battleTimeIsOver() => Time.time - lastTimeWasInBattle >= enemy.battleTimerDuration;
    private bool withInAttackRange()
    {
        return distanceToPlayer() <= enemy.attackDistance && enemy.playerDetected;
    }
    private bool shouldRetreat() => distanceToPlayer() < enemy.minRetreatDistance && enemy.playerDetected;

    private float distanceToPlayer()
    {
        if(player == null) return float.MaxValue;
        return Mathf.Abs(player.position.x - enemy.transform.position.x);
    }

    private int directionToPlayer()
    {
        if(player == null) return 0;
        return player.position.x > enemy.transform.position.x ? 1 : -1;
    }
}
