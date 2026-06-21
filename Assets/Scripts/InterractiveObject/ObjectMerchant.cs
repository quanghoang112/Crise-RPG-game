using UnityEngine;

public class ObjectMerchant : ObjectNPC, IInteractable
{
    private InventoryPlayer inventory;
    private InventoryMerchant merchant;

    [Header("Quest & Dialogue")]
    [SerializeField] private QuestDataSO[] quests;
    [SerializeField] private DialogueLineSO firstDialogueLine;


    protected override void Awake()
    {
        base.Awake();
        merchant = GetComponent<InventoryMerchant>();
    }

    public override void Interact()
    {
        base.Interact();
        ui.merchantUI.SetupMerchantUI(merchant,inventory);
        
        ui.OpenDialogueUI(firstDialogueLine, new DialogueNpcData(rewardNpc, quests,dropManager));

        // ui.OpenQuestUI(quests);
        
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
