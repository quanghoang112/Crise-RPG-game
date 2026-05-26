using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SkillShard : SkillBase
{
    private SkillObject_Shard currentShard;
    private SkillObject_Shard rawShard;
    private EntityHealth playerHealth;

    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private float detonateTime = 2;

    [Header("Moving Shard Upgrade")]
    [SerializeField] private float shardSpeed = 7;

    [Header("Multicast Shard Upgrade")]
    [SerializeField] private int maxCharges =3;
    [SerializeField] private int currentCharges;
    [SerializeField] private bool isCharging;

    [Header("Teleport Shard Upgrade")]
    [SerializeField] private float shardExistDuration = 10;

    [Header("Health Rewind Shard Upgrade")]
    [SerializeField] private float savedHealthPercent;





    protected override void Awake()
    {
        skillType = SkillType.TimeShard;
        currentCharges = maxCharges;
        playerHealth = GetComponentInParent<EntityHealth>();
    }

    public override void TryUseSkill()
    {
        base.TryUseSkill();
        // Debug.Log(CanUseSkill());
        if(!CanUseSkill())
        {
            // Debug.Log("Cant");
            return;
        }

        if(Unlocked(SkillUpgradeType.Shard))
        {
            HandleShardRegular();
            // Debug.Log("Shard");
        }

        if(Unlocked(SkillUpgradeType.ShardMoveToEnemy))
        {
            HandleShardMoving(true);
            Debug.Log(currentShard.canMove);
        }

        if(Unlocked(SkillUpgradeType.ShardTripleCast))
        {
            HandleShardMulticast();
            Debug.Log("Cast");
        }
        if(Unlocked(SkillUpgradeType.ShardTeleport))
        {
            HandleShardTeleport();
            Debug.Log("Teleport");
        }
        if(Unlocked(SkillUpgradeType.ShardTeleportHpRewind))
        {
            HandleShardHealthRewind();
            Debug.Log("Teleport&HpRewind");
        }
    }

    private void HandleShardHealthRewind()
    {
        if(currentShard == null)
        {
            CreateShard();
            savedHealthPercent = playerHealth.GetHealthPercent();
        }
        else
        {
            SwapPlayerAndShard();
            playerHealth.SetHealthToPercent(savedHealthPercent);

            SetSkillOnCooldown();
        }
    }

    private void HandleShardMulticast()
    {
        if(currentCharges <= 0)
            return;
        CreateShard();
        currentShard.MoveTowardsClosestTarget(shardSpeed);
        currentCharges--;

        if(isCharging == false)
            StartCoroutine(ShardRechargeCo());

        
    }

    private IEnumerator ShardRechargeCo()
    {
        isCharging = true;

        while(currentCharges < maxCharges)
        {
            currentCharges++;
            yield return new WaitForSeconds(cooldown);
        }

        isCharging = false;
    }

    private void HandleShardTeleport()
    {
        if(currentShard == null)
        {
            CreateShard();
        }
        else
        {
            Debug.Log("CurrShard not null");
            SwapPlayerAndShard();
            SetSkillOnCooldown();
        }
    }

    private void SwapPlayerAndShard()
    {
        Vector3 shardPosition = currentShard.transform.position;
        Vector3 playerPosition = player.transform.position;

        currentShard.transform.position = playerPosition;
        currentShard.Explode();
        
        player.TeleportPLayer(shardPosition);
    }

    private void HandleShardMoving(bool canMove)
    {
        CreateShard();
        currentShard.MoveTowardsClosestTarget(shardSpeed);
        currentShard.canMove = canMove;

        SetSkillOnCooldown();
    }
    private void HandleShardRegular()
    {
        CreateShard();
        SetSkillOnCooldown();
    } 
    public void CreateShard()
    {
        
        GameObject shard = Instantiate(shardPrefab,transform.position,Quaternion.identity);
        currentShard = shard.GetComponent<SkillObject_Shard>();
        currentShard.SetupShard(this);
    
        if(Unlocked(SkillUpgradeType.ShardTeleport) || Unlocked(SkillUpgradeType.ShardTeleportHpRewind))
            currentShard.OnExplode += ForceCooldown;
    }

    public void CreateRawShard()
    {
        bool canMove = Unlocked(SkillUpgradeType.ShardTeleport) || Unlocked(SkillUpgradeType.ShardTeleportHpRewind);
        // Debug.Log(canMove);
        GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);

        rawShard = shard.GetComponent<SkillObject_Shard>();
        // rawShard.SetupShard(this, detonateTime, canMove, shardSpeed);
        rawShard.SetupShard(this);

        if(this.damageScaleData == null)
            Debug.Log("Shard scale damage is null");
        else
            Debug.Log("Shard is not null");
    
    }

    public float GetDetonateTime()
    {
        if(Unlocked(SkillUpgradeType.ShardTeleport) || Unlocked(SkillUpgradeType.ShardTeleportHpRewind))
            return shardExistDuration;
        return detonateTime;
    }

    private void ForceCooldown()
    {
        if(!OnCooldown())
        {
            SetSkillOnCooldown();
            currentShard.OnExplode -= ForceCooldown;
        }
    }
}
