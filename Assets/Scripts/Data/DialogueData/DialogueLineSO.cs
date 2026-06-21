using UnityEngine;


[CreateAssetMenu(menuName = "RPG Setup/Dialogue Data/New Line Data", fileName = "Line - ")]
public class DialogueLineSO : ScriptableObject
{
    [Header("Dialogue info")]
    public string dialogueGroupName;
    public DialogueSpeakerSO speaker;

    [Header("Text options")]
    [TextArea] public string[] textLines;

    [Header("Choices info")]
    [TextArea] public string playerChoiceAnswer;
    public DialogueLineSO[] choiceLines;
    

    [Header("Dialogue Action")]
    [TextArea] public string actionLine;
    public DialogueActionType actionType;

    public string GetFirstLine() => textLines[0];

    public string GetRandomLine()
    {
        return textLines[Random.Range(0,textLines.Length)];
    }
}
