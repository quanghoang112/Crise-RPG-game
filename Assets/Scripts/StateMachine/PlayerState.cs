using UnityEngine;

public class PlayerState:EntityState
{
    protected Player player;
    protected PlayerSkillManager skillManager;

    public PlayerState(Player player,StateMachine stateMachine, string animBoolName):base(stateMachine,animBoolName)
    {
        this.player = player;
        this.anim = player.anim;
        this.rb = player.rb;
        this.entityStats = player.entityStats;
        this.skillManager = player.skillManager;
    }
    public override void Update()
    {
        base.Update();
        
        if(player.input.Player.Dash.WasPerformedThisFrame() && canDash())
        {
            stateMachine.ChangeState(player.dashState);
            skillManager.dash.SetSkillOnCooldown();
        }
        if(player.input.Player.UltimateSpell.WasPerformedThisFrame() && skillManager.domainExpansion.CanUseSkill())
        {
            if(skillManager.domainExpansion.InstantDomain())
            {
                skillManager.domainExpansion.CreateDomain();
            }
            else
            {
                stateMachine.ChangeState(player.domainState);
            }

            skillManager.domainExpansion.SetSkillOnCooldown();
        }
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void updateAnimationParameters()
    {
        base.updateAnimationParameters();
        anim.SetFloat("yVelocity",player.rb.linearVelocity.y);
    }
    private bool canDash()
    {
        if(skillManager.dash.CanUseSkill() == false)
            return false;
        if(player.wallDetected)
            return false;
        return true;
    }
} 