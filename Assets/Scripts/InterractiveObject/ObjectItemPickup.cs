using UnityEngine;
using UnityEngine.Rendering;

public class ObjectItemPickup : MonoBehaviour
{
    [SerializeField] private ItemDataSO itemData;
    [SerializeField] private Vector2 dropForce = new Vector2(3,10);

    private SpriteRenderer sr => GetComponent<SpriteRenderer>();
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    private Collider2D col => GetComponent<Collider2D>();
    

    // private InventoryBase inventory;


    
    private void OnValidate()
    {

        SetupVisuals();
        
    
    }

    public void SetupItem(ItemDataSO itemData)
    {
        this.itemData = itemData;
        SetupVisuals();

        float xForce = Random.Range(-dropForce.x, dropForce.x);
        rb.linearVelocity = new Vector2(xForce,dropForce.y);
        col.isTrigger = false;
    }

    private void SetupVisuals()
    {
        sr.sprite = itemData.itemIcon;
        gameObject.name = $"ObjectItemPickup - {itemData.itemName}";
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground") && col.isTrigger == false)
        {
            col.isTrigger = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        InventoryItem itemToAdd = new InventoryItem(itemData);

        if(collision.GetComponent<Player>() == null)    return;
        Debug.Log("Player picked up item - " + itemData.itemName);
        
        InventoryPlayer inventory = collision.GetComponent<InventoryPlayer>();
        InventoryStorage storage = inventory.storage;

        if(itemData.itemType == ItemType.Material)
        {
            storage.AddMaterialToStash(itemToAdd);
            Destroy(gameObject);
            return;
        }
        
        if(inventory.CanAddItem(itemToAdd))
        {
            inventory.AddItem(itemToAdd);
            Destroy(gameObject);
        }
    }
}
