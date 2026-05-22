using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player player, StateMachine stateMachine, string AnimBoolName) : base(player, stateMachine, AnimBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.setVelocity(player.moveSpeed * -player.facingDir*player.wallJumpMultiplier, player.jumpForce);
    }
    public override void Update()
    {
        base.Update();
        if(player.rb.linearVelocity.y < 0)
        {
            stateMachine.ChangeState(player.fallState);
        }
        else
        {
            stateMachine.ChangeState(player.jumpState);
        }
    }
}
