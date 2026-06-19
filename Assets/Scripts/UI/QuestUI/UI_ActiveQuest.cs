using UnityEngine;

public class UI_ActiveQuest : MonoBehaviour
{
    private PlayerQuestManager questManager;
    private UI_ActiveQuestSlot[] questSlots;
    private UI_ActiveQuestPreview questPreview;

    private void Awake()
    {
        questManager = Player.instance.questManager;
        questSlots = GetComponentsInChildren<UI_ActiveQuestSlot>(true);
        questPreview = GetComponentInChildren<UI_ActiveQuestPreview>(true);
    
    }

    private void OnEnable()
    {
        questPreview.MakeQuestPreviewEmpty();
        foreach(var questSlot in questSlots)
        {
            questSlot.gameObject.SetActive(false);
        }

        for(var i = 0;i<questManager.activeQuests.Count; i++)
        {
            if(questManager.activeQuests[i] == null)    continue;

            questSlots[i].gameObject.SetActive(true);
            questSlots[i].SetupActiveQuestSlot(questManager.activeQuests[i]);
        }

        if(questManager.activeQuests.Count > 0)//mặc định luon activeQuestPreview đầu tiên
            questSlots[0].SetupPreview();
    }
}
