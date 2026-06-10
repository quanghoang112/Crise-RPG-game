using UnityEngine;
using System.Collections;
using System;

public class Entity : MonoBehaviour
{
    public Rigidbody2D rb{get; private set;}
    public Animator anim{get; private set;}
    public EntitySFX sfx {get;private set;}
    
    protected StateMachine stateMachine;
    private bool isFacingRight = true;
    public int facingDir = 1;

    public event Action onFlipped;

    





    [Header("Collision Detection")]
    public float groundCheckDistance;
    public bool GroundDetected{get; private set;}
    public float wallCheckDistance;
    public bool wallDetected{get; private set;}
    public LayerMask whatIsGround;
    [SerializeField]private Transform primaryWallCheck;
    [SerializeField]private Transform secondaryWallCheck;
    [SerializeField]private Transform groundCheck;

    //Condition var
    private bool isKnocked;
    private Coroutine knockbackCo;
    private Coroutine slowDownCo;


    protected virtual void Awake()
    {
        stateMachine = new StateMachine();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sfx = GetComponent<EntitySFX>();
    }
    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        handleCollisionDetection();
        stateMachine.UpdateLiveState();
        
    }



    public void callAnimationTrigger()
    {
        stateMachine.currentState.CallAnimationTrigger();
    }

    public virtual void EntityDeath()
    {
        
    }

    public virtual void slowDownEntityBy(float duration, float slowMultiplier, bool canOverrideSlowEffect = false)
    {

        if(slowDownCo != null)
        {
            if(canOverrideSlowEffect)
                StopCoroutine(slowDownCo);
            else
                return;
        }
        slowDownCo = StartCoroutine(slowDownEntityCo(duration,slowMultiplier));
    }

    public virtual void StopSlowDownEntityBy()
    {
        slowDownCo = null;
    }

    protected virtual IEnumerator slowDownEntityCo(float duration, float slowMultiplier)
    {
        yield return null;
    }

    public void receiveKnockback(Vector2 knockback, float duration)
    {
        // Debug.Log("Received Knockback: " + knockback + " for duration: " + duration);
        if(knockbackCo != null)
        {
            StopCoroutine(knockbackCo);
        }
        knockbackCo = StartCoroutine(KnockbackCo(knockback, duration));
    }

    private IEnumerator KnockbackCo(Vector2 knockback, float duration)
    {
        isKnocked = true;
        rb.linearVelocity = knockback;
        yield return new WaitForSeconds(duration);

        rb.linearVelocity = Vector2.zero;
        isKnocked = false;
    }
    

    public void setVelocity(float xVelocity, float yVelocity)
    {
        if(isKnocked)
        {
            return;
        }
        rb.linearVelocity= new Vector2(xVelocity,yVelocity);
        handleFlip(xVelocity);
    }
    public void Flip()
    {
        this.transform.Rotate(0f,180f,0f);
        isFacingRight = !isFacingRight;
        facingDir *= -1;
        onFlipped?.Invoke();
    }
    public void handleFlip(float xVelocity)
    {
        if(xVelocity > 0f && !isFacingRight)
        {
            // Debug.Log("flipRight");
            // Debug.Log(xVelocity);
            Flip();
        }
        else if(xVelocity < 0f && isFacingRight)
        {
            // Debug.Log("flipLeft");
            // Debug.Log(xVelocity);
            Flip();
        }
    }
    protected virtual void handleCollisionDetection()
    {
        GroundDetected = Physics2D.Raycast(groundCheck.position,Vector2.down,groundCheckDistance,whatIsGround);
        wallDetected = Physics2D.Raycast(primaryWallCheck.position,facingDir == 1 ? Vector2.right : Vector2.left,wallCheckDistance,whatIsGround)
        && Physics2D.Raycast(secondaryWallCheck.position,facingDir == 1 ? Vector2.right : Vector2.left,wallCheckDistance,whatIsGround);
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0f,-groundCheckDistance,0f));
        Gizmos.DrawLine(primaryWallCheck.position, primaryWallCheck.position + new Vector3(facingDir*wallCheckDistance,0f,0f));
        Gizmos.DrawLine(secondaryWallCheck.position, secondaryWallCheck.position + new Vector3(facingDir*wallCheckDistance,0f,0f));
        // Gizmos.DrawLine(transform.position,transform.position + new Vector3(-wallCheckDistance,0f,0f));
    }
}
