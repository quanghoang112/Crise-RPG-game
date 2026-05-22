using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player player, StateMachine stateMachine, string AnimBoolName) : base(player, stateMachine, AnimBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        HandleWallSlide();
        if(player.input.Player.Jump.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.wallJumpState);
        }
        if(!player.wallDetected)
        {
        stateMachine.ChangeState(player.fallState);
        }
        if(player.GroundDetected)
        {
            stateMachine.ChangeState(player.idleState);
            player.Flip();
        }
    }
    private void HandleWallSlide()
    {
        if(player.moveInput.y <0)
        {
            player.setVelocity(player.moveInput.x, player.rb.linearVelocity.y);
        }
        else
        {
            player.setVelocity(player.moveInput.x, player.rb.linearVelocity.y * player.wallSlideSlowdownMultiplier);
        }
    }
}
