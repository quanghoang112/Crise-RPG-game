using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Grant skill point", fileName = "Item effect data - Grant skill point")]
public class ItemEffect_GrantSkillPoint : ItemEffectDataSO
{
    [SerializeField] private int pointsToAdd;
    public override void ExecuteEffect()
    {
        // base.ExecuteEffect();
        UI ui = FindAnyObjectByType<UI>();

        ui.skillTree.AddSkillPoints(pointsToAdd);
    }
}
