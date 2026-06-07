using UnityEngine;

public class SkillObject_ThrowSword : SkillObject_Base
{
    
    protected SkillThrowSword swordManager;
    
    protected float maxAllowedDistance = 25f;
    protected Transform playerTransform;
    private bool canComeback;
    private float comebackSpeed = 20;

    protected virtual void Update()
    {
        // playerTransform = swordManager.player.transform.root;

        // 1. Tính góc quay dựa trên Vận tốc (Trả về góc tính bằng độ từ -180 đến 180)
        float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;

        // 2. CỘNG THÊM GÓC LỆCH (OFFSET)
        // Nếu mũi kiếm bị chúc xuống đất: Hãy thử cộng thêm 90 hoặc trúng hơn là -90 tùy Sprite gốc
        // Bạn hãy test thay đổi số 90f dưới đây thành -90f hoặc 180f cho đến khi mũi kiếm thẳng băng nhé!
        float spriteOffset = 135f; 

        // 3. Áp dụng góc quay vào thanh kiếm
        transform.rotation = Quaternion.Euler(0, 0, angle + spriteOffset);
    
        HandleComeback();
    
    }

    public void GetSwordBackToPlayer() => canComeback = true;
    public virtual void SetupSword(SkillThrowSword swordManager, Vector2 direction)
    {
        // rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction;

        this.swordManager = swordManager;

        playerTransform = swordManager.transform.root;

        playerStats = swordManager.player.entityStats;
        damageScaleData = swordManager.damageScaleData;
    }

    protected void HandleComeback()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if(distance > maxAllowedDistance)
        {
            // Debug.Log(maxAllowedDistance);
            GetSwordBackToPlayer();
        }

        if(canComeback == false)
            return;
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position,comebackSpeed * Time.deltaTime);

        if(distance < .5f)
            Destroy(gameObject);

    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        StopSword(collision);
        DamageEnemiesInRadius(transform,1);
    }

    protected void StopSword(Collider2D collision)
    {
        rb.simulated = false;
        transform.parent = collision.transform;
    }
}
