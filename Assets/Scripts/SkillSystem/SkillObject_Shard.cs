using System;
using UnityEngine;
using UnityEngine.Splines.ExtrusionShapes;

public class SkillObject_Shard : SkillObject_Base
{
    public event Action OnExplode;

    [SerializeField] private GameObject vfxPrefab;
    private CircleCollider2D col;
    private Transform target;
    
    private float speed;
    public bool canMove;

    private void Awake()
    {
        canMove = false;
        col = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        if(canMove == false)
            return;
        if(canMove == true)
            if(target == null)
                MoveTowardsClosestTarget(speed);
            else
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

    }

    public void MoveTowardsClosestTarget(float speed, Transform newTarget = null)
    {
        target = newTarget != null ? newTarget : FindClosestTarget();
        this.speed = speed;
    }


    public void SetupShard(SkillShard shardManager)
    {

        playerStats = shardManager.player.entityStats;
        damageScaleData = shardManager.damageScaleData;

        Invoke(nameof(Explode),shardManager.GetDetonateTime());
    }

    public void SetupShard(SkillShard shardManager, float detonateTime, bool canMove, float shardSpeed, Transform target = null)
    {

        playerStats = shardManager.player.entityStats;
        damageScaleData = shardManager.damageScaleData;

        Invoke(nameof(Explode),shardManager.GetDetonateTime());

        this.canMove = canMove;
        if(canMove)
            MoveTowardsClosestTarget(shardSpeed,target);
    }

    public void Explode()
    {
        DamageEnemiesInRadius(transform, col.radius);
        Instantiate(vfxPrefab, transform.position, Quaternion.identity);

        OnExplode?.Invoke();

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() == null)
            return;
        Explode();
    }
}
