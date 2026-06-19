using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Quest Data/ New Quest", fileName = "Quest - ")]

public class QuestDataSO : ScriptableObject
{
    public string questSaveId;
    [Space]
    public string questName;
    public QuestType questType;
    [TextArea] public string description;
    [TextArea] public string questGoal;

    public string questTargetId;//Enemy name, NPC name, Item Name
    public int requiredAmount;
    
    [Header("Delivery")]
    public ItemDataSO itemToDeliver;

    [Header("Rewards")]
    public RewardType rewardType;
    public InventoryItem[] rewardItems;
    



    private void OnValidate()
    {
        #if UNITY_EDITOR
            string path = AssetDatabase.GetAssetPath(this);
            questSaveId = AssetDatabase.AssetPathToGUID(path);
        #endif
    }
}
