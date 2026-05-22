using UnityEngine;

public class PlayerDeathState : PlayerState
{
    private Collider2D playerCollider;
    public PlayerDeathState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        playerCollider = player.GetComponent<Collider2D>();
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Player Death");
        stateMachine.swithOffStateMachine();
        
        rb.simulated = false;
    }
    // public override void Update()
    // {
    //     base.Update();
    //     if(triggerCalled)
    //     {
    //         // anim.enabled = false;
    //         rb.simulated = false;
    //     }
    // }
}
