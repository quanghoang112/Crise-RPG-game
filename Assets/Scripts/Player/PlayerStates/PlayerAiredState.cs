using UnityEngine;

public class PlayerAiredState : PlayerState
{
    public PlayerAiredState(Player player, StateMachine stateMachine, string AnimBoolName) : base(player, stateMachine, AnimBoolName)
    {
    }

     public override void Update()
    {
        base.Update();
        if(player.moveInput.x != 0)
        {
            player.setVelocity(player.moveInput.x * player.moveSpeed * player.inAirMoveMultiplier, player.rb.linearVelocity.y);
        }
        if(player.input.Player.Attack.WasPerformedThisFrame())
        {
            stateMachine.ChangeState(player.jumpAttackState);
        }
    }
}
