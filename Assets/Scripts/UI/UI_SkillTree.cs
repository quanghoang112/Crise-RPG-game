using Unity.VisualScripting;
using UnityEngine;

public class UI_SkillTree : MonoBehaviour
{
    public int skillPoints;
    public UI_TreeConnectHandler[] parentNodes;
    public PlayerSkillManager skillManager{get;private set;}

    private void Awake()
    {
        skillManager = FindAnyObjectByType<PlayerSkillManager>();
    }
    private void Start()
    {
        UpdateAllConnections();
    }

    public void RemoveSkillPoints(int cost) => skillPoints -= cost;

    public bool EnoughSkillPoints(int cost) => skillPoints >= cost;

    public void AddSkillPoints (int points) => skillPoints += points;

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
