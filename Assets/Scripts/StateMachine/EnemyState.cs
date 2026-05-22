using UnityEngine;

public class EnemyState : EntityState
{
    protected Enemy enemy;
    public EnemyState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.enemy = enemy;
        this.anim= enemy.anim;
        this.rb = enemy.rb;
        this.entityStats = enemy.entityStats;
    }

    public override void Update()
    {
        base.Update();

    }

    public override void updateAnimationParameters()
    {
        base.updateAnimationParameters();
        
        float battleAnimSpeedMultiplier = enemy.battleMoveSpeed / enemy.moveSpeed;
        anim.SetFloat("moveAnimSpeedMultiplier", enemy.moveAnimSpeedMultiplier);
        anim.SetFloat("xVelocity",enemy.rb.linearVelocity.x);
        anim.SetFloat("battleAnimSpeedMultiplier", enemy.moveAnimSpeedMultiplier * battleAnimSpeedMultiplier);
    }
    
}
