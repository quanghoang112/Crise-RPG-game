using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SkillShard : SkillBase
{
    private SkillObject_Shard currentShard;

    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private float detonateTime = 2;

    [Header("Moving Shard Upgrade")]
    [SerializeField] private float shardSpeed = 7;

    [Header("Multicast Shard Upgrade")]
    [SerializeField] private int maxCharges =3;
    [SerializeField] private int currentCharges;
    [SerializeField] private bool isCharging;

    private void Awake()
    {
        skillType = SkillType.TimeShard;
        currentCharges = maxCharges;
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
        currentShard.SetupShard(detonateTime);
    }

}
