using UnityEngine;

public class UI : MonoBehaviour
{
    public UI_SkillToolTip skillToolTip{get; private set;}
    public UI_ItemToolTip itemToolTip{get; private set;}
    public UI_StatsToolTip statsToolTip{get; private set;}
    public UI_SkillTree skillTree{get; private set;}
    public UI_Craft craftUI{get;private set;}
    public UI_Storage storage {get;private set;}
    [SerializeField] private GameObject[] uiTabs;       // Mảng chứa các Tab con
    private int currentTabIndex = 0;                    // Index của Tab đang hiển thị
    private bool    currActive;

    private void Awake()
    {
        skillToolTip = GetComponentInChildren<UI_SkillToolTip>();
        itemToolTip = GetComponentInChildren<UI_ItemToolTip>();
        statsToolTip = GetComponentInChildren<UI_StatsToolTip>();
        skillTree = GetComponentInChildren<UI_SkillTree>();
        craftUI = GetComponentInChildren<UI_Craft>(true);

        storage = GetComponentInChildren<UI_Storage>(true);
    }

    private void Start()
    {
        gameObject.SetActive(true);
        setAllUnactiveTabs();
    }

    public void SwitchOffAllTooltips()
    {
        itemToolTip.showToolTip(false, null);
        statsToolTip.showToolTip(false,null);
        skillToolTip.showToolTip(false,null);
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

        Debug.Log(currentTabIndex);

        if(number == -1)
            if(currentTabIndex == 0)
            {
                uiTabs[n-1].SetActive(true);
                currentTabIndex = n-1;
            }
            else
            {
                uiTabs[currentTabIndex -1].SetActive(true);
                currentTabIndex -=1;
            }
        else
            if(currentTabIndex == n-1)
            {
                uiTabs[0].SetActive(true);
                currentTabIndex = 0;
            }
            else
            {
                uiTabs[currentTabIndex + 1].SetActive(true);
                currentTabIndex += 1;
            }

    }

    public void setAllUnactiveTabs()
    {
        for(int i = 0 ;i < uiTabs.Length;i++)
        {
            uiTabs[i].SetActive(false);
        }
    }
}
