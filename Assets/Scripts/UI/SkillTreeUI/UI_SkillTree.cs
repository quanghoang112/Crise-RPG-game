using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_SkillTree : MonoBehaviour
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

}
