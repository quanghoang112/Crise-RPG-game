using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR;

public class SkillObject_TimeEcho : SkillObject_Base
{
    [SerializeField] private float wispMoveSpeed = 15f;
    [SerializeField] private GameObject onDeathVfx;
    [SerializeField] private LayerMask whatIsGround;

    public int maxAttacks {get;private set;}
    private bool shouldMoveToPlayer;

    private SkillTimeEcho timeEchoManager;
    private TrailRenderer wispTrail;
    private Transform playerTransform;
    private EntityHealth playerHealth;
    private PlayerSkillManager skillManager;
    private EntityStatusHandler statusHandler;
    private SkillObject_Health echoHealth;

    public void SetupTimeEcho(SkillTimeEcho timeEchoManager, float facingDir)
    {
        this.timeEchoManager = timeEchoManager;
        playerStats = timeEchoManager.player.entityStats;
        damageScaleData = timeEchoManager.damageScaleData;
        maxAttacks = timeEchoManager.GetMaxAttacks();
        anim.SetBool("Attack", maxAttacks > 0);
        playerTransform = timeEchoManager.transform.root;
        playerHealth = timeEchoManager.player.entityHealth;
        skillManager = timeEchoManager.player.skillManager;
        statusHandler = timeEchoManager.player.statusHandler;

        echoHealth = GetComponent<SkillObject_Health>();
        wispTrail = GetComponentInChildren<TrailRenderer>();
        wispTrail.gameObject.SetActive(false);

        if(facingDir == -1) gameObject.transform.Rotate(0f,180f,0f);

        FlipToTarget(facingDir);


        Invoke(nameof(HandleDeath), timeEchoManager.GetEchoDuration());
    }
    private void Update()
    {
        if(shouldMoveToPlayer)
            HandleWispMovement();
        else
        {
            anim.SetFloat("yVelocity", rb.linearVelocity.y);
            StopHorizontalMovement();
        }


        anim.SetFloat("yVelocity",rb.linearVelocity.y);
        StopHorizontalMovement();
    }

    private void HandleWispMovement()
    {
        transform.position = Vector2.MoveTowards(transform.position,playerTransform.position, wispMoveSpeed * Time.deltaTime);

        if(Vector2.Distance(transform.position, playerTransform.position) < .5f)
        {
            HandlePlayerTouch();
            Destroy(gameObject);
        }
    }

    private void HandlePlayerTouch()
    {
        float healAmount = echoHealth.lastDamageTaken * timeEchoManager.GetPercentOfDamageHealed();
        playerHealth.IncreaseHealth(healAmount);

        float amountInSeconds = timeEchoManager.GetCooldownReduceInSeconds();
        skillManager.reduceAllSKillCooldownBy(amountInSeconds);

        if(timeEchoManager.CanRemoveNegativeEffects())
            statusHandler.removeAllNegativeEffects();

    }

    private void FlipToTarget(float facingDir)
    {
        Transform target = FindClosestTarget();

        if(target == null)  return;

        if(target.position.x < transform.position.x && facingDir == 1)
            transform.Rotate(0f,180f,0f);
        if(target.position.x > transform.position.x && facingDir == -1)
            transform.Rotate(0f,180f,0f);
        
    }

    public void HandleDeath()
    {
        Instantiate(onDeathVfx, transform.position,quaternion.identity);
        
        if(timeEchoManager.shouldBeWisp())
        {
            // Debug.Log("Wisp");
            shouldMoveToPlayer = true;
            anim.gameObject.SetActive(false);
            wispTrail.gameObject.SetActive(true);
            rb.simulated = false;
        }
        else
            Destroy(gameObject);


    }

    private void StopHorizontalMovement()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position,Vector2.down,1.5f, whatIsGround);

        if(hit.collider != null)
            rb.linearVelocity = new Vector2(0,rb.linearVelocity.y);
    
    
    }

    public void PerformAttack()
    {
        DamageEnemiesInRadius(targetCheck, 1);

        if(targetGotHit == false)
            return;
        
        bool canDuplicate = UnityEngine.Random.value < timeEchoManager.GetDuplicateChance();
        float xOffset = transform.position.x < lastTarget.position.x ? 1 : -1;
        // timeEchoManager.lastTarget = lastTarget;

        if(canDuplicate)
            timeEchoManager.CreateTimeEcho(lastTarget.position + new Vector3(xOffset,0));
    }
}
