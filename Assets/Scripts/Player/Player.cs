// using System.Numerics;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System;

public class Player : Entity 
{
    public PlayerInputSet input{get; private set;}
    public static event Action onPlayerDeath;    
    public PlayerSkillManager skillManager {get;private set;}
    public PlayerVFX vfx;


#region State Variable
    public PlayerIdleState idleState {get; private set;}
    public PlayerMoveState moveState {get; private set;}
    public PlayerFallState fallState {get; private set;}
    public PlayerJumpState jumpState {get; private set;} 
    public PlayerWallSlideState wallSlideState {get; private set;}
    public PlayerWallJumpState wallJumpState {get; private set;}
    public PlayerDashState dashState {get; private set;}
    public PlayerAttackState basicAttackState {get; private set;}
    public PlayerJumpAttackState jumpAttackState {get; private set;}
    public PlayerDeathState deathState {get; private set;}
    public PlayerCounterAttackState counterAttackState {get; private set;}
#endregion

    public Vector2 moveInput {get; private set;}


    [Header("Movement details")]
    public float moveSpeed = 10f;
    public float jumpForce = 10f;
    [Range(0f,1f)]
    public float inAirMoveMultiplier = 0.2f;
    [Range(0f,1f)]
    public float wallSlideSlowdownMultiplier = 0.3f;
    [Range(0f,1f)]
    public float wallJumpMultiplier = 0.5f;
    // public Vector2 wallJumpForce;
    [Space]
    public float dashSpeed = 20f;
    public float dashDuration = 0.25f;



    [Header("Attack details")]
    public float attackDuration = 0.1f;
    public Vector2 jumpAttackVelocity;
    public Vector2[] attackVelocity;
    public float comboResetTime = 1f;
    private Coroutine queueAttackCo;

    [Header("Counter Attack details")]
    public float counterAttackDuration = 1f;


    protected override void Awake()
    {
        base.Awake();

        input = new PlayerInputSet();
        skillManager = GetComponent<PlayerSkillManager>();
        vfx = GetComponent<PlayerVFX>();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine,"Move");
        fallState = new PlayerFallState(this, stateMachine, "JumpFall");
        jumpState = new PlayerJumpState(this, stateMachine, "JumpFall");
        wallSlideState = new PlayerWallSlideState(this, stateMachine,"WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "JumpFall");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        basicAttackState = new PlayerAttackState(this, stateMachine, "Attack");
        jumpAttackState = new PlayerJumpAttackState(this, stateMachine, "JumpAttack");
        deathState = new PlayerDeathState(this, stateMachine, "Death");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    private void OnEnable()
    {
        input.Enable();        
        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
        // input.Player.Jump.performed += ctx => Debug.Log(ctx.ReadValueAsButton());
        // input.Player.Jump.canceled += ctx => Debug.Log(ctx.ReadValueAsButton());
    }
    private void OnDisable()
    {
        input.Disable();
    }


    protected override IEnumerator slowDownEntityCo(float duration, float slowMultiplier)
    {
        float originalMoveSpeed = moveSpeed;
        float originalJumpForce = jumpForce;
        float originalAnimSpeed = anim.speed;
        float originalWallJumpForce = jumpForce;
        Vector2 originalJumpAttack = jumpAttackVelocity;
        Vector2[] originalAttackVelocity = new Vector2 [attackVelocity.Length];
        Array.Copy(attackVelocity, originalAttackVelocity,attackVelocity.Length);

        float speedMultiplier = 1 - slowMultiplier;

        moveSpeed = moveSpeed * speedMultiplier;
        jumpForce = jumpForce * speedMultiplier;
        anim.speed = anim.speed * speedMultiplier;
        jumpForce = jumpForce * speedMultiplier;
        jumpAttackVelocity = jumpAttackVelocity * speedMultiplier;

        for (int i = 0; i < attackVelocity.Length; i++)
        {
            attackVelocity[i] = attackVelocity[i] * speedMultiplier;
        }

        yield return new WaitForSeconds(duration);

        moveSpeed = originalMoveSpeed;
        jumpForce = originalJumpForce;
        anim.speed = originalAnimSpeed;
        jumpForce = originalWallJumpForce;
        jumpAttackVelocity = originalJumpAttack;

        for (int i = 0; i < attackVelocity.Length; i++)
        {
            attackVelocity[i] = originalAttackVelocity[i];
        }
    }

    public override void EntityDeath()
    {
        base.EntityDeath();

        onPlayerDeath.Invoke();
        stateMachine.ChangeState(deathState);
    }
    public void enterAttackStateWithoutDelay()
    {
        if(queueAttackCo != null)
        {
            StopCoroutine(queueAttackCo);
        }
        queueAttackCo = StartCoroutine(enterAttackStateWithoutDelayCo());
    }

    private IEnumerator enterAttackStateWithoutDelayCo()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(basicAttackState);
    }
}
