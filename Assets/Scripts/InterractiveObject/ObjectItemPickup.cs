using UnityEngine;
using UnityEngine.Rendering;

public class ObjectItemPickup : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private ItemDataSO itemData;

    // private InventoryBase inventory;


    
    private void OnValidate()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = itemData.itemIcon;
        gameObject.name = $"ObjectItemPickup - {itemData.itemName}";
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
