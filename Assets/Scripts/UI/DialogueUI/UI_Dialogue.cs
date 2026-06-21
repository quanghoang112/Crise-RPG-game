// using Microsoft.Unity.VisualStudio.Editor;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class UI_Dialogue : MonoBehaviour
{
    private UI ui;
    private DialogueNpcData npcData;
    private PlayerQuestManager questManager;

    [SerializeField] private Image speakerPortrait;
    [SerializeField] private TextMeshProUGUI speakerName;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI[] dialogueChoices;

    [Space]
    [SerializeField] private float textSpeed = .4f;
    private string fullTextToShow;
    private Coroutine typeTextCo;

    private DialogueLineSO currentLine;
    private DialogueLineSO[] currentChoices;
    private DialogueLineSO selectedChoice;
    private int selectedChoiceIndex;
    private bool waitingToConfirm = false;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        questManager = Player.instance.questManager;
    }

    public void SetupNpcData(DialogueNpcData npcData)
    {
        this.npcData = npcData;
    }

    public void PlayDialogueLine(DialogueLineSO line)
    {
        currentLine = line;
        currentChoices = line.choiceLines;
        selectedChoice = null;
        selectedChoiceIndex = 0;
        HideAllChoices();

        speakerName.text = line.speaker.speakerName;
        speakerPortrait.sprite = line.speaker.speakerPortrait;
        
        
        fullTextToShow = line.actionType == DialogueActionType.None || line.actionType == DialogueActionType.PlayerMakeChoice ? line.GetRandomLine() : line.actionLine;
        // fullTextToShow = line.actionLine;
        typeTextCo = StartCoroutine(TypeTextCo(fullTextToShow));

        // HandleNextAction();
        
        // selectedChoice = null;
    }

    private void HandleNextAction()
    {
        // Debug.Log("asd");
        switch(currentLine.actionType)
        {
            case DialogueActionType.OpenShop:
                ui.SwitchToInGameUI();
                ui.OpenMerchantUI(true);
                break;

            case DialogueActionType.OpenCraft:
                ui.SwitchToInGameUI();
                ui.OpenCraftUI(true);
                break;

            case DialogueActionType.PlayerMakeChoice:
                // Debug.Log("Asd" + waitingToConfirm + selectedChoice);
                // if(!waitingToConfirm)     return;
                if(selectedChoice == null)
                {
                    selectedChoiceIndex = 0;
                    ShowChoices();
                }
                else
                {
                    DialogueLineSO selectedChoice = currentChoices[selectedChoiceIndex];
                    PlayDialogueLine(selectedChoice);
                    // this.selectedChoice = null;
                    // waitingToConfirm = false;
                }
                
                break;

            case DialogueActionType.OpenQuest:
                ui.SwitchToInGameUI();
                ui.OpenQuestUI(npcData.quests);
                break;

            case DialogueActionType.GetQuestReward:
                ui.SwitchToInGameUI();
                questManager.TryGetRewardFrom(npcData.npcRewardType,npcData.dropManager);
                break;

            case DialogueActionType.CloseDialogue:
                ui.SwitchToInGameUI();
                break;

            default:
                Debug.Log("Not implement yet!");
                break;
        }
    }

    public void DialogueInteraction()//tương tác để hiện ra toàn bộ đoạn hội thoại thay vì ngồi đợi kết thúc Coroutine
    {
        if(typeTextCo != null)
        {
            CompleteTyping();
            // Debug.Log("aaa");
            waitingToConfirm = true;
            return;
        }
        if(waitingToConfirm)
        {
            waitingToConfirm = false;
            HandleNextAction();
        }
    }

    private void CompleteTyping()
    {
        if(typeTextCo != null)
        {
            StopCoroutine(typeTextCo);
            dialogueText.text = fullTextToShow; 
            typeTextCo = null;
        }
    }

    private IEnumerator TypeTextCo(string text)
    {
        dialogueText.text = "";

        foreach(char letter in text)
        {
            dialogueText.text = dialogueText.text + letter;
            yield return new WaitForSeconds(1-textSpeed); //textSpeed càng lớn đợi càng ít, => chữ xuất hiện càng nhanh
        
        }

        waitingToConfirm = true;
        typeTextCo = null;
    }

    private void ShowChoices()
    {
        HideAllChoices();
        // Debug.Log("Asd");

        for(int i = 0;i< dialogueChoices.Length; i++)
        {
            if(i<currentChoices.Length)
            {
                DialogueLineSO choice = currentChoices[i];
                string choiceText = choice.playerChoiceAnswer; 


                dialogueChoices[i].gameObject.SetActive(true);
                dialogueChoices[i].text = selectedChoiceIndex == i ? $"<color=yellow> {i+1}. {choiceText}"
                : $"{i + 1}. {choiceText}";

                if(choice.actionType == DialogueActionType.GetQuestReward && questManager.HasCompletedQuest() == false)
                    dialogueChoices[i].gameObject.SetActive(false);
            }
        }

        selectedChoice = currentChoices[selectedChoiceIndex];
        
        waitingToConfirm = true;// cho phép thực hiện HandleNextAction
    }

    private void HideAllChoices()
    {
        foreach(var obj in dialogueChoices)
        {
            obj.gameObject.SetActive(false);
        }
    }

    public void NavigateChoice(int direction)
    {
        if(currentChoices == null || currentChoices.Length <= 1)
            return;

        selectedChoiceIndex = selectedChoiceIndex + direction;
        selectedChoiceIndex = Mathf.Clamp(selectedChoiceIndex, 0, currentChoices.Length - 1);
        ShowChoices();
    }
}
