using UnityEngine;
using UnityEngine.InputSystem;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject[] uiElements;
    public bool alternativeInput {get; private set;}
    private PlayerInputSet input;

    #region  UI Components
    public UI_SkillToolTip skillToolTip{get; private set;}
    public UI_ItemToolTip itemToolTip{get; private set;}
    public UI_StatsToolTip statsToolTip{get; private set;}
    public UI_SkillTree skillTree{get; private set;}
    public UI_Craft craftUI{get;private set;}
    public UI_Storage storageUI {get;private set;}
    public UI_Merchant merchantUI{get;private set;}
    public UI_InGame inGameUI {get; private set;}
    public UI_Options optionsUI {get; private set;}
    #endregion

    [SerializeField] private GameObject[] uiTabs;       // Mảng chứa các Tab con
    private int currentTabIndex = 0;                    // Index của Tab đang hiển thị
    private bool    currActive;

    private void Awake()
    {
        skillToolTip = GetComponentInChildren<UI_SkillToolTip>();
        itemToolTip = GetComponentInChildren<UI_ItemToolTip>();
        statsToolTip = GetComponentInChildren<UI_StatsToolTip>();
        skillTree = GetComponentInChildren<UI_SkillTree>(true);
        craftUI = GetComponentInChildren<UI_Craft>(true);
        merchantUI = GetComponentInChildren<UI_Merchant>(true);
        inGameUI = GetComponentInChildren<UI_InGame>(true);
        optionsUI = GetComponentInChildren<UI_Options>(true);
        storageUI = GetComponentInChildren<UI_Storage>(true);
    }

    private void Start()
    {
        gameObject.SetActive(true);
        setAllUnactiveTabs();

        skillTree.UnlockDefaultSkills();
    }

    public void SetupControlsUI (PlayerInputSet inputSet)
    {
        input = inputSet;

        input.UI.CanvasToggle.performed += ctx => ToggleCanvas();
        input.UI.CanvasTab.performed += ctx => changeTab(ctx.ReadValue<float>());
        
        input.UI.AlternativeInput.performed += ctx => alternativeInput = true;
        input.UI.AlternativeInput.canceled += ctx => alternativeInput = false;
    
        input.UI.OptionUIToggle.performed += ctx => 
        {
            foreach(var element in uiElements)
            {
                if (element.activeSelf)
                {
                    Time.timeScale = 1;
                    SwitchToInGameUI();
                    return;
                }
            }
            
            Time.timeScale = 0;
            OpenOptionsUI();    
        };
    
    }

    public void OpenOptionsUI()
    {
        foreach (var element in uiElements)
            element.gameObject.SetActive(false);

        HideAllTooltips();
        StopPlayerControls(true);
        optionsUI.gameObject.SetActive(true);
    }

    public void SwitchToInGameUI()
    {
        foreach (var element in uiElements)
            element.gameObject.SetActive(false);

        HideAllTooltips();
        StopPlayerControls(false);
        inGameUI.gameObject.SetActive(true);
    }

    public void HideAllTooltips()
    {
        itemToolTip.showToolTip(false, null);
        statsToolTip.showToolTip(false,null);
        skillToolTip.showToolTip(false,null);
    }

    private void StopPlayerControls(bool stopControls)
    {
        if(stopControls)
            input.Player.Disable();
        else
            input.Player.Enable();
    }

    private void StopPlayerControlsIfNeeded()
    {
        foreach(var element in uiElements)
        {
            if(element.activeSelf)
            {
                StopPlayerControls(true);
                return;
            }
        }
    }

    public void ToggleCanvas()
    {
        if (gameObject != null)
        {
            // Đảo ngược trạng thái hoạt động (Active) hiện tại của Canvas
            // currActive = !isActive;
            // gameObject.SetActive(currActive);
            // setAllUnactiveTabs();

            bool isActive = uiTabs[currentTabIndex].activeSelf;            
            uiTabs[currentTabIndex].SetActive(!isActive);
            uiTabs[currentTabIndex].transform.SetAsLastSibling();

            currActive = !isActive;

            if(currActive == false) HideAllTooltips();
            else    SetTooltipsAboveOtherElements();
            
            // StopPlayerControls(currActive);
            StopPlayerControlsIfNeeded();
            // Mẹo nhỏ cho Game Designer: Xử lý đóng/mở chuột khi ẩn hiện UI
            // if (!isActive) // Nếu chuẩn bị MỞ Canvas
            // {
            //     Cursor.lockState = CursorLockMode.None;
            //     Cursor.visible = true;
            // }
            // else // Nếu chuẩn bị ĐÓNG Canvas để quay lại chơi game
            // {
            //     Cursor.lockState = CursorLockMode.Locked;
            //     Cursor.visible = false;
            // }
    
        }
    }
    public void changeTab(float number)
    {
        if(!currActive) return;
        int n = uiTabs.Length;

        setAllUnactiveTabs();

        // Debug.Log(currentTabIndex);

        if(number == -1)
            if(currentTabIndex == 0)
            {
                // uiTabs[n-1].SetActive(true);
                currentTabIndex = n-1;
            }
            else
            {
                // uiTabs[currentTabIndex -1].SetActive(true);
                currentTabIndex -=1;
            }
        else
            if(currentTabIndex == n-1)
            {
                // uiTabs[0].SetActive(true);
                currentTabIndex = 0;
            }
            else
            {
                // uiTabs[currentTabIndex + 1].SetActive(true);
                currentTabIndex += 1;
            }

            uiTabs[currentTabIndex].SetActive(true);
            uiTabs[currentTabIndex].transform.SetAsLastSibling();
            SetTooltipsAboveOtherElements();

    }

    public void setAllUnactiveTabs()
    {
        for(int i = 0 ;i < uiTabs.Length;i++)
        {
            uiTabs[i].SetActive(false);
        }
    }

    public void OpenStorageUI(bool openStorageUI)
    {
        storageUI.gameObject.SetActive(openStorageUI);
        StopPlayerControls(openStorageUI);
    }

    public void OpenMerchantUI(bool openMerchantUI)
    {
        merchantUI.gameObject.SetActive(openMerchantUI);
        StopPlayerControls(openMerchantUI);
    }

    private void SetTooltipsAboveOtherElements()
    {
        itemToolTip.transform.SetAsLastSibling();
        skillToolTip.transform.SetAsLastSibling();
        statsToolTip.transform.SetAsLastSibling();
    }
}
