using UnityEngine;


//Visual các item có thể craft, cùng loại với visual trên category icon
//các weapon này được visual ở UI_CraftSlot - UICraftPreview
public class UI_CraftListButton : MonoBehaviour
{
    [SerializeField] private ItemListDataSO craftData;
    private UI_CraftSlot[] craftSlots;

    public void SetCraftSlots(UI_CraftSlot[] craftSlots) => this.craftSlots = craftSlots;

    public void UpdateCraftSlots()
    {
        if(craftData == null)
        {
            Debug.Log("U need to assign craft list data!");
            return;
        }

        foreach(var slot in craftSlots)
        {
            slot.gameObject.SetActive(false);
        }

        for (int i = 0;i<craftData.itemList.Length; i++)
        {
            ItemDataSO itemData = craftData.itemList[i];

            craftSlots[i].gameObject.SetActive(true);
            craftSlots[i].SetupButton(itemData);
        }
    }
}
