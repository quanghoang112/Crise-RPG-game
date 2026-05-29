using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player player, StateMachine stateMachine, string AnimBoolName) : base(player, stateMachine, AnimBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        if(player.input.Player.Jump.WasPerformedThisFrame())
            stateMachine.ChangeState(player.jumpState);
        else if(!player.GroundDetected && !player.wallDetected)
            stateMachine.ChangeState(player.fallState);
        if(player.input.Player.Attack.WasPerformedThisFrame())
            stateMachine.ChangeState(player.basicAttackState);
        if(player.input.Player.CounterAttack.WasPerformedThisFrame())
            stateMachine.ChangeState(player.counterAttackState);
        if(player.input.Player.RangeAttack.WasPerformedThisFrame() && skillManager.throwSword.CanUseSkill())
            stateMachine.ChangeState(player.throwSwordState);
    }
    // public void handleJump()
    // {
    //     if(player.input.Player.Jump.WasPerformedThisFrame())
    //     {
    //         player.setVelocity(player.rb.linearVelocity.x, player.jumpForce);
    //     } 
    // }
}
