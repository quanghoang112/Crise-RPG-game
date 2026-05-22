using UnityEngine;

public class PlayerJumpAttackState : PlayerState
{
    private bool touchGround;
    private int jumpAttackDir;
    public PlayerJumpAttackState(Player player, StateMachine stateMachine, string AnimBoolName) : base(player, stateMachine, AnimBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        touchGround = false;
        jumpAttackDir = player.moveInput.x != 0?(int)Mathf.Sign(player.moveInput.x):player.facingDir;
        player.setVelocity(player.jumpAttackVelocity.x*jumpAttackDir, player.jumpAttackVelocity.y);
    }
    public override void Update()
    {
        base.Update();
        if(player.GroundDetected && !touchGround)
        {
            touchGround = true;
            player.anim.SetTrigger("JumpAttackTrigger");
            player.setVelocity(0, player.rb.linearVelocity.y);
        }

        if(triggerCalled && player.GroundDetected)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
