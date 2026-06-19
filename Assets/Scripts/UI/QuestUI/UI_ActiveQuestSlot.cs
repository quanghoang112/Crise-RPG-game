// using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_ActiveQuestSlot : MonoBehaviour
{
    private QuestData questInSlot;
    private UI_ActiveQuestPreview questPreview;

    [SerializeField] private TextMeshProUGUI questName;
    [SerializeField] private Image[] questRewardPreview;

    private void OnEnable()
    {
        questPreview = transform.root.GetComponentInChildren<UI_ActiveQuestPreview>();
    }

    public void SetupActiveQuestSlot(QuestData questToSetup)
    {
        questInSlot = questToSetup;

        questName.text = questInSlot.questDataSO.questName;
        InventoryItem[] rewards = questInSlot.questDataSO.rewardItems;

        foreach (var previewIcon in questRewardPreview)
        {
            previewIcon.gameObject.SetActive(false);
        }
        
        
        for (int i = 0; i < rewards.Length; i++)
        {
            if(rewards[i] == null)  continue;

            Image preview = questRewardPreview[i];

            preview.gameObject.SetActive(true);
            preview.sprite = rewards[i].itemData.itemIcon;
            preview.GetComponentInChildren<TextMeshProUGUI>().text = rewards[i].stackSize.ToString();
        }
    }

    public void SetupPreview()
    {
        questPreview.SetupQuestPreview(questInSlot);
    }
}
