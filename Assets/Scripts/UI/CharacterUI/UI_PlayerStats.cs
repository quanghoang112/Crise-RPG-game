using UnityEngine;

public class UI_PlayerStats : MonoBehaviour
{
    private UI_StatsSlot[] uiStatsSlots;
    private InventoryPlayer inventory;

    private void Awake()
    {
        uiStatsSlots = GetComponentsInChildren<UI_StatsSlot>();

        inventory = FindAnyObjectByType<InventoryPlayer>();
        inventory.OnInventoryChange += UpdateStatsUI; 
    }

    private void Start()
    {
        UpdateStatsUI();
    }

    private void UpdateStatsUI()
    {
        foreach (var statSlot in uiStatsSlots)
        {
            statSlot.UpdateStatValue();
        }
    }
}
