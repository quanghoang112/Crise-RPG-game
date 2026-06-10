using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    public EnemyIdleState idleState;
    public EnemyMoveState moveState;
    public EnemyAttackState attackState;
    public EnemyBattleState battleState;
    public EnemyDeathState deathState;
    public EnemyStunnedState stunnedState;
    public EntityStats entityStats {get;private set;}
    public EntityDropManager dropManager{get;private set;}
    [Range(0f,2f)]
    public float moveAnimSpeedMultiplier = 1f;


    [Header("Stunned details")]
    public float stunnedDuration = 1f;
    public Vector2 stunnedVelocity;
    public bool canBeStunned = false;


    [Header("Player detection details")]
    public float playerCheckDistance = 5f;
    public bool playerDetected = false;
    public LayerMask whatIsPlayer;
    
    [Header("battle details")]
    public float battleMoveSpeed;
    public float attackDistance=5f;
    public float battleTimerDuration = 2f;
    public float minRetreatDistance =1f;
    public Vector2 retreatVelocity;

    [Header("Movement details")]
    public float moveTimerDuration = 2f;
    public float moveSpeed = 5f;
    public Transform playerCheck;
    
    public Transform player{get;private set;}
    public float activeSlowMultiplier {get; private set;} = 1;
    

    protected override void Awake()
    {
        base.Awake();
        entityStats = GetComponent<EntityStats>();
        dropManager = GetComponent<EntityDropManager>();
    
    }
    protected override void Start()
    {
        base.Start();
        // stateMachine.Initialize(idleState);
    }
    protected override void Update()
    {
        base.Update();
    }
    private void OnEnable()
    {
        Player.onPlayerDeath += handlePlayerDeath;
    }
    private void OnDisable()
    {
        Player.onPlayerDeath -= handlePlayerDeath;
    }

    public float GetMoveSpeed() => moveSpeed * activeSlowMultiplier;
    public float GetBattleMoveSpeed() => battleMoveSpeed * activeSlowMultiplier;
    protected override IEnumerator slowDownEntityCo(float duration, float slowMultiplier)
    {
        

        activeSlowMultiplier = 1 - slowMultiplier;
        
        anim.speed = anim.speed * activeSlowMultiplier;

        yield return new WaitForSeconds(duration);

        
    }

    public override void StopSlowDownEntityBy()
    {
        activeSlowMultiplier = 1f;
        anim.speed = 1f;
        base.StopSlowDownEntityBy();

    }

    public void enableCounterWindow(bool enable) => canBeStunned = enable; 
    private void handlePlayerDeath()
    {
        stateMachine.ChangeState(idleState);
    }
    public override void EntityDeath()
    {
        base.EntityDeath();
        stateMachine.ChangeState(deathState);
    }

    public void tryEnterBattleState(Transform player)
    {
        if(stateMachine.currentState == battleState || stateMachine.currentState == attackState)
            return;
        this.player = player;
        stateMachine.ChangeState(battleState);
    }
    
    public Transform GetPlayerReference()
    {
        if(player == null)
        {
            player = PlayerDetection().transform;
        }
        return player;
    }
    public RaycastHit2D PlayerDetection()
    {
        RaycastHit2D hit = 
            Physics2D.Raycast(playerCheck.position, facingDir == 1 ? Vector2.right : Vector2.left, playerCheckDistance, whatIsPlayer | whatIsGround);
        if(hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
        return default;
        return hit;
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color=Color.yellow;
        Gizmos.DrawLine(playerCheck.position, playerCheck.position + new Vector3(playerCheckDistance*facingDir,0f,0f));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(playerCheck.position, playerCheck.position + new Vector3(attackDistance*facingDir,0f,0f));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(playerCheck.position, playerCheck.position + new Vector3(minRetreatDistance*facingDir,0f,0f));
    }
    protected override void handleCollisionDetection()
    {
        base.handleCollisionDetection();
        playerDetected = PlayerDetection();
    }
}
