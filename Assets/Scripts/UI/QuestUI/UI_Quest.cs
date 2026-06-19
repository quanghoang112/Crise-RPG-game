using UnityEngine;

public class UI_Quest : MonoBehaviour
{
    [SerializeField] private UI_ItemSlotParent inventorySlots;
    [SerializeField] private UI_QuestPreview questPreview;
    
    private UI_QuestSlot[] questSlots;
    public PlayerQuestManager questManager {get;private set;}

    private void Awake()
    {
        questSlots = GetComponentsInChildren<UI_QuestSlot>(true);

        questManager = Player.instance.questManager;
    }

    private void OnEnable()
    {
        questPreview?.MakeQuestPreviewEmpty();
    }

    public void SetupQuestUI(QuestDataSO[] questsToSetup)
    {
        foreach(var slot in questSlots)
            slot.gameObject.SetActive(false);

        for(int i = 0; i<questsToSetup.Length; i++)
        {
            questSlots[i].gameObject.SetActive(true);
            questSlots[i].SetupQuestSlot(questsToSetup[i]);
        }

        inventorySlots.UpdateSlots(Player.instance.inventory.itemList);
    
        UpdateQuestList();
    }

    public void UpdateQuestList()
    {
        foreach(var slot in questSlots)
        {
            if(slot.questInSlot == null) continue;

            if(slot.gameObject.activeSelf && CanTakeQuest(slot.questInSlot) == false)
                slot.gameObject.SetActive(false);
        }
    }

    private bool CanTakeQuest(QuestDataSO questToCheck)
    {
        bool questActive = questManager.QuestIsActive(questToCheck);
        bool questCompleted = questManager.QuestIsCompleted(questToCheck);
        return questActive == false && questCompleted == false;
    }



    public UI_QuestPreview GetQuestPreview() => questPreview;
}
