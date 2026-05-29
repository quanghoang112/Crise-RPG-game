using System.Collections.Generic;
using UnityEngine;

public class SkillDomain : SkillBase
{

    [SerializeField] private GameObject domainPrefab;

    [Header("Slowing Down Upgrade")]
    [SerializeField] private float slowDownPercent = .8f;
    [SerializeField] private float slowDownDomainDuration = 5f;

    [Header("Spell Casting Upgrade")]
    [SerializeField] private int spellsToCast = 10;
    [SerializeField] private float spellCastingDomainSlowDown = 1f;
    [SerializeField] private float spellCastingDomainDuration = 8f;
    private float spellCastTimer;
    private float spellsPerSecond;



    [Header("Domain details")]
    public float maxDomainSize = 20;
    public float expandSpeed = 3;

    private List<Enemy> trappedTargets = new List<Enemy>();
    private Transform currentTarget;

    public void CreateDomain()
    {
        // Debug.Log("Ultimate");

        spellsPerSecond = spellsToCast / GetDomainDuration();

        GameObject domain = Instantiate(domainPrefab, transform.position, Quaternion.identity);
        domain.GetComponent<SkillObject_DomainExpansion>().SetupDomain(this);
    }

    public void DoSpellCasting()
    {

        spellCastTimer -= Time.deltaTime;
        // CreateDomain();
        if(currentTarget == null)
            currentTarget = FindTargetInDomain();

        if(currentTarget != null && spellCastTimer < 0)
        {
            CastSpell(currentTarget);
            spellCastTimer = 1/spellsPerSecond;
            currentTarget = null;
        }
    }

    private void CastSpell(Transform target)
    {
        Debug.Log("Cast Spell");
        // CreateDomain();
        if(upgradeType == SkillUpgradeType.DomainEchoSpam)
        {
            Vector3 offset = Random.value < .5f ? new Vector2(1,0) : new Vector2(-1,0);
            skillManager.timeEcho.CreateTimeEcho(target.position + offset);
        }
        if(upgradeType == SkillUpgradeType.DomainShardSpam)
        {
            skillManager.shard.CreateRawShard(target,true);
        }
    }

    private Transform FindTargetInDomain()
    {
        if(trappedTargets.Count == 0)
            return null;

        int randomIndex = Random.Range(0, trappedTargets.Count);
        Transform target = trappedTargets[randomIndex].transform;

        if(target == null)
        {
            trappedTargets.RemoveAt(randomIndex);
            return null;
        }

        return target;
    }

    public float GetDomainDuration()
    {
        if(upgradeType == SkillUpgradeType.DomainSlowingDown)
            return slowDownDomainDuration;
        else 
            return spellCastingDomainDuration;
    }

    public float GetSlowPercentage()
    {
        if(upgradeType == SkillUpgradeType.DomainSlowingDown)
            return slowDownPercent;
        else
            return spellCastingDomainSlowDown;
    }

    public bool InstantDomain()
    {
        return upgradeType != SkillUpgradeType.DomainEchoSpam
        && upgradeType != SkillUpgradeType.DomainShardSpam;
    }

    public void AddTarget (Enemy targetToAdd)
    {
        trappedTargets.Add(targetToAdd);
    }

    public void ClearTargets()
    {
        foreach(var enemy in trappedTargets)
            enemy.StopSlowDownEntityBy();

        trappedTargets = new List<Enemy>();
    }
}
