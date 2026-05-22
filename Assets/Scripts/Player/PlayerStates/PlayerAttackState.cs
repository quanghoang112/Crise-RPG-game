using UnityEditor.ShaderGraph.Internal;
using System.Collections;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    private float attackDuration;
    private int firstAttackIndex = 0;
    private int attackDir;
    private int comboIndex = 0;
    private int comboLength = 3;
    private float lastAttackTime;
    private bool comboAttackQueued=false;
    public PlayerAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        if(player.attackVelocity.Length != comboLength)
        {
            Debug.LogError("Adjust attack velocity array length to match combo length.");
            comboLength = player.attackVelocity.Length;
        }
    }
    public override void Enter()
    {
        base.Enter();
        resetComboIfNeeded();
        SyncAttackSpeed();

        lastAttackTime = Time.time;
        player.anim.SetInteger("BasicAttackIndex", comboIndex);

        attackDir = player.moveInput.x != 0?(int)Mathf.Sign(player.moveInput.x):player.facingDir;
        
        applyAttackVelocity();
        
    }
    public override void Update()
    {
        base.Update();
        handleAttackVelocity();


        if(player.input.Player.Attack.WasPerformedThisFrame())
        {
            queueNextAttack();
        }
        if(triggerCalled)
        {
            handleStateExit();
        }
    }
    public override void Exit()
    {
        base.Exit();
        comboIndex++;
    }

    private void handleStateExit()
    {
        if(comboAttackQueued) 
            {
                comboAttackQueued = false;
                player.anim.SetBool(AnimBoolName, false);
                player.enterAttackStateWithoutDelay();
            }
            else
                stateMachine.ChangeState(player.idleState);
    }
    private void handleAttackVelocity()
    {
        attackDuration -= Time.deltaTime;
        if(attackDuration <= 0)
        // {
            player.setVelocity(0, player.rb.linearVelocity.y);
        // }
    }

    private void queueNextAttack()
    {
        if(comboIndex < comboLength-1)
        {
            comboAttackQueued = true;
        }
    }
    private void applyAttackVelocity()
    {
        attackDuration = player.attackDuration;
        player.setVelocity(player.attackVelocity[comboIndex].x*attackDir, player.attackVelocity[comboIndex].y);
    }
    private void resetComboIfNeeded()
    {
        if(Time.time - lastAttackTime > player.comboResetTime)
        {
            comboIndex = firstAttackIndex;
        }
        if (comboIndex >= comboLength)
        {
            comboIndex = firstAttackIndex;
        }
    }
}
