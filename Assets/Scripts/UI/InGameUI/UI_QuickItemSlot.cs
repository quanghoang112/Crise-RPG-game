using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_QuickItemSlot : UI_ItemSlot
{
    private Button button;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private int slotNumber;

    protected override void Awake()
    {
        base.Awake();
        button = GetComponent<Button>();
    }


    public void SimulateButtonFeedback()
    {
        if (button == null || !button.interactable) return;

        // 1. Giả lập click chuột để lấy hiệu ứng hình ảnh
        EventSystem.current.SetSelectedGameObject(button.gameObject);
        ExecuteEvents.Execute(button.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);

        // 2. KHẮC PHỤC: Xóa bỏ focus ngay lập tức để bàn phím trả về trạng thái lắng nghe logic game
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void SetupQuickSlotItem(InventoryItem itemToPass)
    {
        inventory.SetQuickItemInSlot(slotNumber,itemToPass);
    }

    public void UpdateQuickSlotUI(InventoryItem currentItemInSlot)
    {
        // SimulateButtonFeedback();
        if(currentItemInSlot == null || currentItemInSlot.itemData == null)
        {
            itemIcon.sprite = defaultSprite;
            itemStackSize.text = "";
            return;
        }

        itemIcon.sprite = currentItemInSlot.itemData.itemIcon;
        itemStackSize.text = currentItemInSlot.stackSize.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        // base.OnPointerDown(eventData);
        ui.inGameUI.OpenQuickItemOptions(this,rect);
    }
}
