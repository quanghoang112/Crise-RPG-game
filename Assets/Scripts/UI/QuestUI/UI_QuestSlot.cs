// using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using JetBrains.Annotations;

public class UI_QuestSlot : MonoBehaviour
{
    private UI_QuestPreview questPreview;
    public QuestDataSO questInSlot{get; private set;}
    

    [SerializeField] private TextMeshProUGUI questName;
    [SerializeField] private Image[] rewardQuickPreviewSlots;

    private void Awake()
    {
        questName = GetComponentInChildren<TextMeshProUGUI>();
        // rewardQuickPreviewSlots = GetComponentsInChildren<Image>();
    
    }

    public void SetupQuestSlot(QuestDataSO questDataSO)
    {
        questPreview = transform.root.GetComponentInChildren<UI_Quest>().GetQuestPreview();

        questInSlot = questDataSO;
        questName.text = questInSlot.questName;

        foreach(var previewIcon in rewardQuickPreviewSlots)
        {
            previewIcon.gameObject.SetActive(false);
        }

        for(int i = 0;i < questInSlot.rewardItems.Length; i++)
        {
            if(questInSlot.rewardItems[i] == null || questDataSO.rewardItems[i].itemData == null)   continue;

            Image slot = rewardQuickPreviewSlots[i];

            slot.gameObject.SetActive(true);
            slot.sprite = questInSlot.rewardItems[i].itemData.itemIcon;
            slot.GetComponentInChildren<TextMeshProUGUI>().text = questInSlot.rewardItems[i].stackSize.ToString();
        }
    }

    public void UpdateQuestPreview()
    {
        questPreview.SetupQuestPreview(questInSlot);
    }

}

