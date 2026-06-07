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
    public UI ui{get;private set;}
    public EntityHealth entityHealth {get; private set;}
    public EntityStatusHandler statusHandler{get;private set;}
    public PlayerCombat playerCombat{get;private set;}
    public InventoryPlayer inventory {get;private set;}
    public PlayerStats entityStats{get;private set;}


#region State Variable
    public PlayerThrowSwordState throwSwordState{get; private set;}
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
    public PlayerDomainExpansionState domainState {get;private set;}
#endregion

    public Vector2 moveInput {get; private set;}
    public Vector2 mousePosition {get; private set;}

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

    [Header("Ultimate ability details")]
    public float riseSpeed = 25f;
    public float riseMaxDistance = 3f;
    

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

        ui = FindAnyObjectByType<UI>();
        skillManager = GetComponent<PlayerSkillManager>();
        vfx = GetComponent<PlayerVFX>();
        entityHealth = GetComponent<EntityHealth>();
        statusHandler = GetComponent<EntityStatusHandler>();
        playerCombat = GetComponent<PlayerCombat>();
        inventory = GetComponent<InventoryPlayer>();
        entityStats = GetComponent<PlayerStats>();
        
        input = new PlayerInputSet();
        ui.SetupControlsUI(input);

        throwSwordState = new PlayerThrowSwordState(this, stateMachine,"Throw");
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
        domainState = new PlayerDomainExpansionState(this,stateMachine,"JumpFall");
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


        input.Player.Spell.performed += ctx => skillManager.shard.TryUseSkill();
        input.Player.Spell.performed += ctx => skillManager.timeEcho.TryUseSkill();
        
        
        
        // input.Player.Jump.performed += ctx => Debug.Log(ctx.ReadValueAsButton());
        // input.Player.Jump.canceled += ctx => Debug.Log(ctx.ReadValueAsButton());
        input.Player.Mouse.performed += ctx => mousePosition = ctx.ReadValue<Vector2>();
        input.Player.Interact.performed += ctx => TryInteract();

        input.Player.QuickItemSlot1.performed += ctx => inventory.TryUseQuickItemInSlot(1);
        // input.Player.QuickItemSlot1.performed += ctx =>Debug.Log("asdasd");
        input.Player.QuickItemSlot2.performed += ctx => inventory.TryUseQuickItemInSlot(2);
    }
    private void OnDisable()
    {
        input.Disable();
    }

    private void TryInteract()
    {
        Transform closest = null;
        float closestDistance = Mathf.Infinity;
        Collider2D[] objectsAround = Physics2D.OverlapCircleAll(transform.position, 1f);

        foreach (var target in objectsAround)
        {
            IInteractable interactable = target.GetComponent<IInteractable>();
            if(interactable == null)    continue;

            float distance = Vector2.Distance(transform.position, target.transform.position);

            if(distance < closestDistance)
            {
                closestDistance = distance;
                closest = target.transform;
            }
        }

        if(closest == null) return;

        closest.GetComponent<IInteractable>().Interact();
    }

    public void TeleportPLayer(Vector3 position) => transform.position = position;


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
