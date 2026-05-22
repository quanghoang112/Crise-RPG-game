using UnityEngine;
using UnityEngine.Splines.ExtrusionShapes;

public class SkillObject_Shard : SkillObject_Base
{
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

    public void MoveTowardsClosestTarget(float speed)
    {
        target = FindClosestTarget();
        this.speed = speed;
    }


    public void SetupShard(float detonateTime)
    {
        Invoke(nameof(Explode),detonateTime);
    }

    private void Explode()
    {
        DamageEnemiesInRadius(transform, col.radius);
        Instantiate(vfxPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() == null)
            return;
        Explode();
    }
}
