using UnityEngine;

public class UI : MonoBehaviour
{
    public UI_SkillToolTip skillToolTip;

    private void Awake()
    {
        skillToolTip = GetComponentInChildren<UI_SkillToolTip>();
    }

    public void ToggleCanvas()
    {
        if (gameObject != null)
        {
            // Đảo ngược trạng thái hoạt động (Active) hiện tại của Canvas
            bool isActive = gameObject.activeSelf;
            gameObject.SetActive(!isActive);

            // Mẹo nhỏ cho Game Designer: Xử lý đóng/mở chuột khi ẩn hiện UI
            if (!isActive) // Nếu chuẩn bị MỞ Canvas
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else // Nếu chuẩn bị ĐÓNG Canvas để quay lại chơi game
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
    
        }
    }
}
