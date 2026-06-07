using UnityEngine;

public class Chest : MonoBehaviour, IDamagable
{
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    private Animator anim => GetComponentInChildren<Animator>();
    private EntityVFX fx => GetComponent<EntityVFX>();
    private EntityDropManager dropManager => GetComponent<EntityDropManager>();

    [Header("Open details")]
    [SerializeField] private Vector2 knockback;
    [SerializeField] private bool canDropItems = true;
    
    public bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        if(canDropItems == false)
            return false;
        
        canDropItems = false;
        dropManager?.DropItems();
        anim?.SetBool("Open", true);
        rb.linearVelocity = knockback; // Example knockback effect
        rb.angularVelocity = Random.Range(-200f, 200f); // Random spin for visual effect
        
        
        fx?.PlayOnDamageVFX();
        return true;
    }
}
