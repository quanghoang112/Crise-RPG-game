using UnityEngine;

public class SkillBase : MonoBehaviour
{
    private Player _player;
    public Player player
    {
        get
        {
            // Nếu chưa tìm thấy Player, thì mới đi tìm
            if (_player == null)
            {
                _player = GetComponentInParent<Player>();
            }
            return _player;
        }
    }

    private PlayerSkillManager _skillManager;
    public PlayerSkillManager skillManager
    {
        get
        {
            if(_skillManager == null)
            {
                _skillManager = GetComponentInParent<PlayerSkillManager>();
            }
            return _skillManager;
        }
    }
    public DamageScaleData damageScaleData{get; private set;}

    [Header("General details")]
    [SerializeField] protected SkillType skillType;
    [SerializeField] protected SkillUpgradeType upgradeType;
    [SerializeField] protected float cooldown;
    [SerializeField] private float lastTimeUsed;

    protected virtual void Awake()
    {
        lastTimeUsed = lastTimeUsed - cooldown;
        // player = GetComponentInParent<Player>();
        // skillManager = GetComponentInParent<PlayerSkillManager>();
        damageScaleData = new DamageScaleData();
    }

    public virtual void TryUseSkill()
    {
        
    }

    public void SetSkillUpgrade(UpgradeData upgradeData)
    {
        this.upgradeType = upgradeData.upgradeType;
        cooldown = upgradeData.cooldown;
        damageScaleData = upgradeData.damageScale;
        // Debug.Log(damageScaleData.burnDamageScale);
    }

    public virtual bool CanUseSkill()
    {
        if(upgradeType == SkillUpgradeType.None)
            return false;
        if(OnCooldown())  
        {
            Debug.Log("On cooldown");
            return false;
        }
        return true;
    }

    protected bool Unlocked(SkillUpgradeType upgradeToCheck) => upgradeType == upgradeToCheck;

    protected bool OnCooldown() => Time.time < lastTimeUsed + cooldown;
    public void SetSkillOnCooldown() => lastTimeUsed = Time.time;
    public void ReduceCooldownBy (float cooldownReduction) => lastTimeUsed = lastTimeUsed + cooldownReduction;
    public void ResetCooldown() => lastTimeUsed=Time.time;
}
