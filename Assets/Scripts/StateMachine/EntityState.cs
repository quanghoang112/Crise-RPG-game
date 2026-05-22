using UnityEngine;

public abstract class EntityState
{
    protected StateMachine stateMachine;
    protected string AnimBoolName;
    protected Animator anim;
    protected Rigidbody2D rb;
    protected EntityStats entityStats;

    protected float stateTimer;
    protected bool triggerCalled;

    public EntityState(StateMachine stateMachine, string animBoolName)
    {
        this.stateMachine=stateMachine;
        this.AnimBoolName=animBoolName;
    }

    public virtual void Update()
    {
        // Debug.Log("I run state: " + AnimBoolName);
        stateTimer -= Time.deltaTime;
        updateAnimationParameters();
    }
    public virtual void Enter()
    {
        // Debug.Log("Entering state: " + AnimBoolName);
        anim.SetBool(AnimBoolName, true);
        triggerCalled = false;
        //everytime state will be changed, the enter function will be called, so we can do some initialization here
    
    }

    public virtual void Exit()
    {
        // Debug.Log("Exiting state: " + AnimBoolName);
        anim.SetBool(AnimBoolName,false);
        //everytime state will be changed, the exit function will be called, so we can do some cleanup here
    }
    public void CallAnimationTrigger()
    {
        triggerCalled = true;
    }
    public virtual void updateAnimationParameters()
    {
        // Debug.Log("Updating animation parameters for state: " + AnimBoolName);
    }

    public void SyncAttackSpeed()
    {
        float attackSpeed = entityStats.offenseStats.attackSpeed.GetValue();
        anim.SetFloat("AttackSpeedMultiplier",attackSpeed);
    }
}
