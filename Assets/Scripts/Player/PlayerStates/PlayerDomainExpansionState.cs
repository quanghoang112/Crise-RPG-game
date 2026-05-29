using UnityEngine;

public class PlayerDomainExpansionState : PlayerState
{
    private Vector2 originalPosition;
    private float originalGravity;
    private float maxDistanceToGoUp;
    
    // private float maxDistanceToGoUp;
    private bool isLevitating;
    private bool createdDomain;

    public PlayerDomainExpansionState(Player player, StateMachine stateMachine, string AnimBoolName) : base(player, stateMachine, AnimBoolName)
    {
    }


    public override void Enter()
    {
        base.Enter();

        originalPosition = player.transform.position;
        originalGravity = rb.gravityScale;
        maxDistanceToGoUp = GetAvailiableRiseDistance();

        player.setVelocity(0, player.riseSpeed);

        player.entityHealth.SetCanTakeDamage(false);
    }

    public override void Update()
    {
        base.Update();

        if(Vector2.Distance(originalPosition,player.transform.position)>= maxDistanceToGoUp && isLevitating == false)
            Levitate();

        if(isLevitating)
        {
            skillManager.domainExpansion.DoSpellCasting();
            if(stateTimer < 0)
            {
                rb.gravityScale = originalGravity;
                isLevitating = false;
                stateMachine.ChangeState(player.idleState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();

        createdDomain = false;
        player.entityHealth.SetCanTakeDamage(true);
    }

    private void Levitate()
    {
        isLevitating = true;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;

        stateTimer = skillManager.domainExpansion.GetDomainDuration();

        if(createdDomain == false)
        {
            createdDomain = true;
            skillManager.domainExpansion.CreateDomain();
        }
    }

    private float GetAvailiableRiseDistance()
    {
        RaycastHit2D hit =
            Physics2D.Raycast(player.transform.position, Vector2.up, player.riseMaxDistance, player.whatIsGround);

        return hit.collider != null ? hit.distance - 1 : player.riseMaxDistance;
    }
}
