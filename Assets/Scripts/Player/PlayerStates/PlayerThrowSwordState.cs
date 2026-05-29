using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerThrowSwordState : PlayerState
{

    private Camera mainCamera;

    public PlayerThrowSwordState(Player player, StateMachine stateMachine, string animBoolName): base(player,stateMachine,animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();

        skillManager.throwSword.EnableDots(true);

        if(mainCamera != Camera.main)
            mainCamera = Camera.main;
    }

    public override void Update()
    {
        base.Update();

        Vector2 dirToMouse = DirectionToMouse();

        player.setVelocity(0, rb.linearVelocity.y);
        player.handleFlip(dirToMouse.x);

        skillManager.throwSword.PredictTrajectory(dirToMouse);


        if(player.input.Player.Attack.WasPressedThisFrame())
        {
            
            anim.SetBool("ThrowPerformed",true);

            skillManager.throwSword.EnableDots(false);
            skillManager.throwSword.ConfirmTrajectory(dirToMouse);
        }
        if(player.input.Player.RangeAttack.WasReleasedThisFrame()|| triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();

        skillManager.throwSword.EnableDots(false);
        anim.SetBool("ThrowPerformed",false);
    }

    private Vector2 DirectionToMouse()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 worldMousePosition = mainCamera.ScreenToWorldPoint(player.mousePosition);

        Vector2 direction = worldMousePosition - playerPosition;
        return direction.normalized;
    }
}
