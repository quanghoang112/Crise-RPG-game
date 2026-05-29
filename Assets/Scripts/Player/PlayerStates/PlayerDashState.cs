using UnityEngine;

public class PlayerDashState : PlayerState
{
    private int dashDir;
    public PlayerDashState(Player player, StateMachine stateMachine, string AnimBoolName) : base(player, stateMachine, AnimBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        player.vfx.ImageEchoEffect(player.dashDuration);
        skillManager.dash.OnStartEffect();

        stateTimer = player.dashDuration;
        dashDir = player.moveInput.x != 0?(int)Mathf.Sign(player.moveInput.x):player.facingDir;
        CancelDashIfNeeded();
        // Implementation for dash enter
        player.entityHealth.SetCanTakeDamage(false);
    }
    public override void Update()
    {
        base.Update();
        CancelDashIfNeeded();
        
        player.setVelocity(dashDir * player.dashSpeed, 0);
        // Implementation for dash update
        if(stateTimer <= 0)
        {
            if(player.GroundDetected)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.fallState);
        }
    }
    public override void Exit()
    {
        base.Exit();

        skillManager.dash.OnEndEffect();
        // Implementation for dash exit
        player.setVelocity(0,0);
        player.entityHealth.SetCanTakeDamage(true);
    }

    private void CancelDashIfNeeded()
    {
        if(player.wallDetected)
        {
            if(player.GroundDetected)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.fallState);
        }
    }
}
 