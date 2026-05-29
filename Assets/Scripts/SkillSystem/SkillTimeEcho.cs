using UnityEngine;

public class SkillTimeEcho : SkillBase
{
    [SerializeField] private GameObject timeEchoPrefab;
    [SerializeField] private float timeEchoDuration;
    // public Transform lastTarget;
    [Header("Attack Upgrades")]
    [SerializeField] private int maxAttacks = 3;
    [SerializeField] private float duplicateChance =.3f;

    [Header("Heal wisp Upgrade")]
    [SerializeField] private float damagePercentHealed = .3f;
    [SerializeField] private float cooldownReducedInSeconds;

    public float GetPercentOfDamageHealed()
    {
        if(shouldBeWisp() == false)
            return 0;
        
        return damagePercentHealed;
    }

    public float GetCooldownReduceInSeconds()
    {
        if(upgradeType != SkillUpgradeType.TimeEchoCooldownWisp)
            return 0;
        return cooldownReducedInSeconds;
    }

    public bool CanRemoveNegativeEffects()
    {
        return upgradeType == SkillUpgradeType.TimeEchoCleanseWisp;
    }

    public int GetMaxAttacks()
    {
        if(upgradeType == SkillUpgradeType.TimeEchoSingleAttack || upgradeType == SkillUpgradeType.TimeEchoChanceToDuplicate)
            return 1;
        if(upgradeType == SkillUpgradeType.TimeEchoMultiAttack)
            return maxAttacks;
        return 0;
    }

    public bool shouldBeWisp()
    {
        return upgradeType == SkillUpgradeType.TimeEchoCleanseWisp
        || upgradeType == SkillUpgradeType.TimeEchoCooldownWisp
        || upgradeType == SkillUpgradeType.TimeEchoHealWisp;
    }


    public float GetDuplicateChance()
    {
        if(upgradeType != SkillUpgradeType.TimeEchoChanceToDuplicate)
            return 0;
        return duplicateChance;
    }
    public override void TryUseSkill()
    {
        base.TryUseSkill();
        if(!CanUseSkill())
            return;
        // if(Unlocked(SkillUpgradeType.TimeEcho))
        // {
            CreateTimeEcho();
            SetSkillOnCooldown();
        // }
    }

    public float GetEchoDuration()
    {
        return timeEchoDuration;
    }
    public void CreateTimeEcho(Vector3? targetPosition = null)
    {
        Vector3 position  = targetPosition ?? transform.position;

        GameObject timeEcho = Instantiate(timeEchoPrefab, position, Quaternion.identity);
        timeEcho.GetComponent<SkillObject_TimeEcho>().SetupTimeEcho(this,player.facingDir);
    }
}
