using UnityEngine;

public enum SkillUpgradeType
{
    None,

    
    Dash,
    DashCloneOnStart,
    DashCloneOnStartAndArrival,
    DashShardOnStart,
    DashShardStartAndArrival,

    //Shard
    Shard,
    ShardMoveToEnemy,
    ShardTripleCast,
    ShardTeleport,
    ShardTeleportHpRewind,

    //SwordThrow
    SwordThrow,
    SwordThrowSpin,
    SwordThrowPierce,
    SwordThrowBounce,

    //TimeEcho
    TimeEcho,
    TimeEchoSingleAttack,
    TimeEchoMultiAttack,
    TimeEchoChanceToDuplicate,
    TimeEchoHealWisp,
    TimeEchoCleanseWisp,
    TimeEchoCooldownWisp,


    //Domain
    DomainSlowingDown,
    DomainEchoSpam,
    DomainShardSpam
}
