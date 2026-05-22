using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    
    
    public PlayerMoveState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if(player.moveInput.x == 0 || player.wallDetected)
        {
            stateMachine.ChangeState(player.idleState);
        }
        // handleFlip();
        handleMovement();
    }
    
    private void handleMovement()
    {
        player.setVelocity(player.moveInput.x*player.moveSpeed, player.rb.linearVelocity.y);
    }
    

}
