using UnityEngine;

// Effect data Special
[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Heal Effect", fileName = "Item effect data - heal")]
public class ItemEffect_Heal : ItemEffectDataSO
{
    [SerializeField] private float healPercent = .1f;

    public override void ExecuteEffect()
    {
        base.ExecuteEffect();

        Player player = FindAnyObjectByType<Player>();

        float healAmount = player.entityStats.GetMaxHealth() * healPercent;
        player.entityHealth.IncreaseHealth(healAmount);
    
    }
}
