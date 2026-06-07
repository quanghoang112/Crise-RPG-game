using UnityEngine;

public class ObjectBlacksmith : ObjectNPC, IInteractable
{

    protected Animator anim;
    private InventoryPlayer inventory;
    private InventoryStorage storage;    
    
    protected override void Awake()
    {
        base.Awake();
        anim = GetComponentInChildren<Animator>();
        storage = GetComponent<InventoryStorage>();

        anim.SetBool("IsBlacksmith",true);
    }
    public void Interact()
    {
        ui.storageUI.SetupStorage(storage);
        ui.craftUI.SetupCraftUI(storage);
        // ui.storage.gameObject.SetActive(true);

        ui.OpenStorageUI(true);
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
