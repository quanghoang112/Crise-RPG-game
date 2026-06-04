using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Refund all skills", fileName = "Item effect data - Refund skill tree")]
public class ItemEffect_RefundSkillTree : ItemEffectDataSO
{
    public override void ExecuteEffect()
    {
        base.ExecuteEffect();

        UI ui = FindAnyObjectByType<UI>();


        ui.skillTree.RefundAllSkills();
    }
}
