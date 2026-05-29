using UnityEngine;
using UnityEngine.Rendering;

public class ObjectItemPickup : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private ItemDataSo itemData;

    private InventoryItem itemToAdd;
    private InventoryBase inventory;


    private void Awake()
    {
        itemToAdd = new InventoryItem(itemData);
    }

    private void OnValidate()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = itemData.itemIcon;
        gameObject.name = $"ObjectItemPickup - {itemData.itemName}";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player picked up item - " + itemData.itemName);
        
        inventory = collision.GetComponent<InventoryBase>();

        if(inventory != null && inventory.CanAddItem())
        {
            inventory.AddItem(itemToAdd);
            Destroy(gameObject);
        }
    }
}
