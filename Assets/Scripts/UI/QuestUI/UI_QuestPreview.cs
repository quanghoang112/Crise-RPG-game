using TMPro;
using UnityEngine;

public class UI_QuestPreview : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questName;
    [SerializeField] private TextMeshProUGUI questDescription;
    [SerializeField] private TextMeshProUGUI questGoal;
    [SerializeField] private UI_QuestRewardSlot[] questReward;

    [SerializeField] private GameObject[] additionalObjects;
    private UI_Quest questUI;
    private QuestDataSO previewQuest;
    public void SetupQuestPreview(QuestDataSO questDataSO)
    {
        questUI = transform.root.GetComponentInChildren<UI_Quest>();
        previewQuest = questDataSO;

        EnableAdditionalObject(true);
        EnableQuestRewardObjects(false);

        questName.text = questDataSO.questName;
        questDescription.text = questDataSO.description;
        questGoal.text = questDataSO.questGoal;

        for (int i  = 0;i<questDataSO.rewardItems.Length; i++)
        {
            if(questDataSO.rewardItems[i] == null || questDataSO.rewardItems[i].itemData == null)   continue;

            InventoryItem itemPreview = new InventoryItem(questDataSO.rewardItems[i].itemData);
            itemPreview.stackSize = questDataSO.rewardItems[i].stackSize;
            
            questReward[i].gameObject.SetActive(true);
            questReward[i].UpdateSlot(itemPreview);
        }
    }

    public void AcceptQuestBTN()
    {
        MakeQuestPreviewEmpty();

        questUI.questManager.AcceptQuest(previewQuest);
        questUI.UpdateQuestList();
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
