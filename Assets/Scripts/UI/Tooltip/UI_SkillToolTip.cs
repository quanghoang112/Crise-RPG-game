using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_ToolTip
{
    private UI_SkillTree skillTree;

    private UI ui;

    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillCooldown;
    [SerializeField] private TextMeshProUGUI skillRequirement;

    [Space]
    [SerializeField] private string metConditionHex = "#CFBE2C";
    [SerializeField] private string notMetConditionHex = "#FF455D";
    [SerializeField] private string importantInfoHex = "#9BB3E3";
    private string lockedSkillText = "You're taken a different path - this skill is now locked!";

    private Coroutine textEffectCo;

    protected override void Awake()
    {
        base.Awake();
        ui = GetComponentInParent<UI>();
        skillTree = ui.GetComponentInChildren<UI_SkillTree>(true);

    }

    public override void showToolTip(bool show, RectTransform targetRect)
    {
        base.showToolTip(show, targetRect);
    }

    public void showToolTip(bool show, RectTransform targetRect,Skill_DataSO skillData, UI_TreeNode node)
    {
        base.showToolTip(show, targetRect);

        if(show == false)   return;

        skillName.text = skillData.displayName;
        skillDescription.text = skillData.description;
        skillCooldown.text = $"Cooldown: {skillData.upgradeData.cooldown}s";

        if(node == null)
        {
            skillRequirement.text = "";
            return;
        }

        string skillLockedText = $"<Color={importantInfoHex}>- {lockedSkillText} </color>";

        skillRequirement.text = node.isLocked ? skillLockedText:GetRequirements(skillData.cost, node.neededNodes, node.conflictNodes);
    }

    public void lockedSkillEffect()
    {
        StopLockedSkillEffect();

        textEffectCo = StartCoroutine(TextBlinkEffectCo(skillRequirement, .15f, 3));
    }

    public void StopLockedSkillEffect()
    {
        if(textEffectCo != null)
            StopCoroutine(textEffectCo);
        
    }

    private string GetRequirements(int skillCost, UI_TreeNode[] neededNodes, UI_TreeNode[] conflictNodes)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("Requirements:");

        string costColor = skillTree.EnoughSkillPoints(skillCost) ? metConditionHex : notMetConditionHex;

        sb.AppendLine($"<color={costColor}>- {skillCost} skill point(s) </color>");

        foreach(var node in neededNodes)
        {
            if(node == null)    continue;
            string nodeColor = node.isUnlocked ? metConditionHex:notMetConditionHex;
            sb.AppendLine($"<Color={nodeColor}>- {node.skillData.displayName} </color>");
        }

        if(conflictNodes.Length <= 0)
            return sb.ToString();

        sb.AppendLine();
        sb.AppendLine($"<color={importantInfoHex}>- Locks out:</color>");

        foreach(var node in conflictNodes)
        {
            if(node == null)    continue;
            // string nodeColor = node.isUnlocked ? metConditionHex:notMetConditionHex;
            sb.AppendLine($"<Color={importantInfoHex}>- {node.skillData.displayName} </color>");
        }
        return sb.ToString();
    }

    private string GetColoredText(string color, string text)
    {
        return $"<Color={color}>- {text}</color>";
    }

    private IEnumerator TextBlinkEffectCo (TextMeshProUGUI text, float blinkInterval, int blinkCount)
    {
        for(int i = 0;i < blinkCount; i++)
        {
            text.text = GetColoredText(notMetConditionHex, lockedSkillText);
            yield return new WaitForSeconds(blinkInterval);

            text.text = GetColoredText(importantInfoHex, lockedSkillText);
            yield return new WaitForSeconds(blinkInterval);
        }
    }

}
