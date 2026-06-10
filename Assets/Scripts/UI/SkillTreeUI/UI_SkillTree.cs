using System.Linq;
using Mono.Cecil;
using TMPro;
using Unity.GraphToolkit.Editor;
using Unity.VisualScripting;
using UnityEngine;

public class UI_SkillTree : MonoBehaviour, ISaveable
{
    public int skillPoints;
    private UI_TreeNode[] allTreeNodes;
    [SerializeField] private TextMeshProUGUI skillPointText;
    public UI_TreeConnectHandler[] parentNodes;
    public PlayerSkillManager skillManager{get;private set;}

    private void Awake()
    {
    }
    private void Start()
    {
        UpdateAllConnections();
        // UnlockDefaultSkills();
        UpdateSkillPointsUI();
    }

    private void UpdateSkillPointsUI()
    {
        skillPointText.text = skillPoints.ToString();
        
    }

    public void UnlockDefaultSkills()
    {
        allTreeNodes = GetComponentsInChildren<UI_TreeNode>(true);
        skillManager = FindAnyObjectByType<PlayerSkillManager>();

        foreach(var node in allTreeNodes)
            node.UnlockDefaultSkills();
    }


    public void RemoveSkillPoints(int cost) 
    {
        skillPoints -= cost;
        UpdateSkillPointsUI();
    }

    public bool EnoughSkillPoints(int cost) => skillPoints >= cost;

    public void AddSkillPoints (int points) 
    {
        skillPoints += points;
        UpdateSkillPointsUI();
    }

    [ContextMenu("Reset skill points")]
    public void RefundAllSkills()
    {
        UI_TreeNode[] skillNodes = GetComponentsInChildren<UI_TreeNode>();
        
        foreach (var node in skillNodes)
            node.Refund();
    }

    [ContextMenu("Update all connections")]
    public void UpdateAllConnections()
    {
        foreach (var node in parentNodes)
        {
            node.UpdateAllConnections();
        }
    }

    public void SaveData(ref GameData data)
    {
        Debug.Log("Saving skill tree data...");
        data.skillPoints = skillPoints;
        data.skillTreeUI.Clear();
        data.skillUpgrades.Clear();

        foreach(var node in allTreeNodes)
        {
            string skillName = node.skillData.displayName;
            data.skillTreeUI[skillName] = node.isUnlocked;
        }

        foreach(var skill in skillManager.allSkills)
        {
            data.skillUpgrades[skill.GetSkillType()] = skill.GetUpgradeType();
        }
    }

    public void LoadData(GameData data)
    {
        skillPoints = data.skillPoints == -1 ? skillPoints: data.skillPoints;

        foreach(var node in allTreeNodes)
        {
            string skillName = node.skillData.displayName;

            if(data.skillTreeUI.TryGetValue(skillName, out bool unlocked) && unlocked)
                node.UnlockWithSaveData();
        }

        foreach (var skill in skillManager.allSkills)
        {
            if(data.skillUpgrades.TryGetValue(skill.GetSkillType(), out SkillUpgradeType upgradeType))
            {
                var upgradeNode = allTreeNodes.FirstOrDefault(node => node.skillData.upgradeData.upgradeType == upgradeType);

                if(upgradeNode != null)
                    skill.SetSkillUpgrade(upgradeNode.skillData);
            }
        }
    }

}
