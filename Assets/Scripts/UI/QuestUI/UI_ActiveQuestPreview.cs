using TMPro;
using UnityEngine;

public class UI_ActiveQuestPreview : MonoBehaviour
{
    // private PlayerQuestManager questManager;

    [SerializeField] private TextMeshProUGUI questName;
    [SerializeField] private TextMeshProUGUI questDescription;
    [SerializeField] private TextMeshProUGUI progress;
    [SerializeField] private UI_QuestRewardSlot[] questReward;

    [SerializeField] private GameObject[] additionalObjects;

    // private void OnEnable()
    // {
    //     questManager = Player.instance.questManager;
    // }
    public void SetupQuestPreview(QuestData questData)
    {
        EnableAdditionalObject(true);
        EnableQuestRewardObjects(false);

        questName.text = questData.questDataSO.questName;
        questDescription.text = questData.questDataSO.description;
        progress.text = $"{questData.questDataSO.questGoal} ({questData.currentAmount}/{questData.questDataSO.requiredAmount})";

        for (int i  = 0;i<questData.questDataSO.rewardItems.Length; i++)
        {
            if(questData.questDataSO.rewardItems[i] == null || questData.questDataSO.rewardItems[i].itemData == null)   continue;

            InventoryItem itemPreview = new InventoryItem(questData.questDataSO.rewardItems[i].itemData);
            itemPreview.stackSize = questData.questDataSO.rewardItems[i].stackSize;
            
            questReward[i].gameObject.SetActive(true);
            questReward[i].UpdateSlot(itemPreview);
        }
    }

    

    public void MakeQuestPreviewEmpty()
    {
        questName.text ="";
        questDescription.text = "";

        EnableAdditionalObject(false);
        EnableQuestRewardObjects(false);

    }

    private void EnableAdditionalObject(bool enable)
    {
        foreach(var obj in additionalObjects)
            obj.SetActive(enable);
    }

    private void EnableQuestRewardObjects(bool enable)
    {
        foreach(var obj in questReward)
            obj.gameObject.SetActive(enable);
    }

}
