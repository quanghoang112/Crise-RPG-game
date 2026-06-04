// using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.GraphToolkit.Editor;

public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI ui;
    private RectTransform rect;
    private UI_SkillTree skillTree;
    private UI_TreeConnectHandler connectHandler;


    [Header("Unlock details")]
    public UI_TreeNode[] neededNodes;
    public UI_TreeNode[] conflictNodes;
    public bool isUnlocked;
    public bool isLocked;


    public Skill_DataSO skillData;
    [SerializeField] private string skillName;
    [SerializeField] private int cost;
    [SerializeField] private Image skillIcon;
    [SerializeField] private Color skillLockedColor;
    private Color lastColor;
    private string lockedColorHex ="#828282";
    private string baseColor ="#FFFFFF";

    private void OnValidate()
    {
        if(skillData == null)
            return;
        skillName = skillData.displayName;
        cost = skillData.cost;
        skillIcon.sprite = skillData.icon;
        gameObject.name = $"UI_TreeNode - {skillName}";
    }

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        skillTree = GetComponentInParent<UI_SkillTree>();
        connectHandler = GetComponent<UI_TreeConnectHandler>();
        UpdateIconColor(skillLockedColor);
        
        // if(skillData.unlockedByDefault)
        //     Unlock();

    }

    private void Start()
    {
        if(skillData.unlockedByDefault)
            Unlock();
    }

    private void Unlock()
    {
        isUnlocked = true;
        Color color=GetColorByHex(baseColor);
        UpdateIconColor(color);
        skillTree.RemoveSkillPoints(skillData.cost);
        LockConflictNodes();
        connectHandler.UnlockConnectionImage(true);

        skillTree.skillManager.GetSkillByType(skillData.skillType).SetSkillUpgrade(skillData.upgradeData);
    }
    private bool canBeUnlocked()
    {
        if(isUnlocked || isLocked)
            return false;
        
        if(!skillTree.EnoughSkillPoints(skillData.cost))
            return false; 
        foreach (var node in neededNodes)
        {
            if(node.isUnlocked == false)
                return false;
        }

        foreach (var node in conflictNodes)
        {
            if(node.isUnlocked)
                return false;
        }
        return true;
    }
    private void UpdateIconColor(Color color)
    {
        if(skillIcon  == null)
        {
            return;
        }
        lastColor = skillIcon.color;
        skillIcon.color = color;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter");
        ui.skillToolTip.showToolTip(true,rect,this);

        if(!isUnlocked ||isLocked)
        {
            Color color = Color.white * .9f; color.a = 1f;
            UpdateIconColor(color);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Exit");
        ui.skillToolTip.showToolTip(false,rect,this);

        if(!isUnlocked || isLocked)
            UpdateIconColor(lastColor);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        // Debug.Log("Down");
        if(canBeUnlocked())
        {
            Debug.Log("unlocked");
            Unlock();
        }
        else if(isLocked)
            // Debug.Log("cant be unlocked");
            ui.skillToolTip.lockedSkillEffect();
    }

    private void LockConflictNodes()
    {
        foreach(var node in conflictNodes)
        {
            node.isLocked = true;
            node.LockChildNodes();
        }
    }

    public void LockChildNodes()
    {
        isLocked = true;
        UI_TreeNode[] children = connectHandler.GetChildNodes();
        
        foreach(var node in children)
        {
            
            node.LockChildNodes();
        }
    }

    private Color GetColorByHex(string hexNumber)
    {
        UnityEngine.ColorUtility.TryParseHtmlString(hexNumber, out Color myColor);
        return myColor;
        
    }

    public void Refund()
    {
        if(isUnlocked == false || skillData.unlockedByDefault)
            return;
        isUnlocked = false;
        isLocked = false;
        UpdateIconColor(GetColorByHex(lockedColorHex));

        skillTree.AddSkillPoints(skillData.cost);
        connectHandler.UnlockConnectionImage(false);
    }
}
