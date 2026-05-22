using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        player.setVelocity(0, player.rb.linearVelocity.y);
    }
    public override void Update()
    {
        base.Update();
        if(player.moveInput.x == player.facingDir && player.wallDetected)
        {
            return;        
        }
        //if player press the move button, we will change to move state
        if (player.moveInput.x != 0)
        {
            stateMachine.ChangeState(player.moveState);
        }
    }
}
