using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private PlayerCombat combat;
    private bool counterSomebody;
    public PlayerCounterAttackState(Player player, StateMachine stateMachine,string animBoolName) : base(player, stateMachine, animBoolName)
    {
        combat = player.GetComponent<PlayerCombat>();
    }

    public override void Enter()
    {
        base.Enter();
        counterSomebody = combat.CounterAttackPerformed();
        stateTimer = player.counterAttackDuration;
        if(counterSomebody)
        {
            anim.SetTrigger("CounterAttackPerformed");
        }
    }
    public override void Update()
    {
        base.Update();
        // Debug.Log(counterSomebody);
        // if(counterSomebody)
        // {
        // }
        if(triggerCalled)
        {
            Debug.Log("Counter attack finished");
            stateMachine.ChangeState(player.idleState);
        }
        if(stateTimer <= 0 && !counterSomebody)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
