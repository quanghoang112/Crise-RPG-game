using UnityEngine;

public class ObjectMerchant : ObjectNPC, IInteractable
{
    private InventoryPlayer inventory;
    private InventoryMerchant merchant;

    [Header("Quest & Dialogue")]
    [SerializeField] private QuestDataSO[] quests;


    protected override void Awake()
    {
        base.Awake();
        merchant = GetComponent<InventoryMerchant>();
    }

    public override void Interact()
    {
        base.Interact();
        ui.OpenQuestUI(quests);
        // ui.merchantUI.SetupMerchantUI(merchant,inventory);
        // ui.OpenMerchantUI(true);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        inventory = player.GetComponent<InventoryPlayer>();
        merchant.SetInventory(inventory);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        ui.HideAllTooltips();
        ui.OpenMerchantUI(false);
    }
}
