using UnityEngine;

public class PlayerFallState : PlayerAiredState
{
    public PlayerFallState(Player player, StateMachine stateMachine, string AnimBoolName) : base(player, stateMachine, AnimBoolName)
    {
    }
    public override void Update()
    {
        base.Update();
        if(player.GroundDetected)
        {
            stateMachine.ChangeState(player.idleState);
        }
        if(player.wallDetected)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
    }
     
}
