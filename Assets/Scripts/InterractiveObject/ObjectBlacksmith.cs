using UnityEngine;

public class ObjectBlacksmith : ObjectNPC, IInteractable
{

    protected Animator anim;
    private InventoryPlayer inventory;
    private InventoryStorage storage;    

    [Header("Quest & Dialogue")]
    [SerializeField] private QuestDataSO[] quests;
    [SerializeField] private DialogueLineSO firstDialogueLine;

    
    protected override void Awake()
    {
        base.Awake();
        anim = GetComponentInChildren<Animator>();
        storage = GetComponent<InventoryStorage>();

        anim.SetBool("IsBlacksmith",true);
    }
    public override void Interact()
    {
        base.Interact();
        
        ui.storageUI.SetupStorage(storage);
        ui.craftUI.SetupCraftUI(storage);
        // ui.storage.gameObject.SetActive(true);

        // ui.OpenStorageUI(true);
        ui.OpenDialogueUI(firstDialogueLine, new DialogueNpcData(npcRewardType:rewardNpc, quests, dropManager));
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        inventory = player.GetComponent<InventoryPlayer>();
        storage.SetInventory(inventory);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);

        ui.HideAllTooltips();
        ui.OpenStorageUI(false);
    }

}
