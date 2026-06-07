using System.Collections.Generic;
using UnityEngine;

public class PlayerDropManager : EntityDropManager
{
    [Header("Player drop details")]
    [Range(0f,100f)]
    [SerializeField] private float chanceToLooseItem = 50f;
    private InventoryPlayer inventory;

    private void Awake()
    {
        inventory = GetComponent<InventoryPlayer>();
    }

    protected override void Update()
    {
        // base.Update();
    }

    public override void DropItems()
    {
        // base.DropItems();

        List<InventoryItem> inventoryCopy = new List<InventoryItem>(inventory.itemList);
        List<InventoryEquipmentSlots> equipCopy = new List<InventoryEquipmentSlots>(inventory.equipList);

        foreach (var item in inventoryCopy)
        {
            if(Random.Range(0, 100) < chanceToLooseItem)
            {
                CreateItemDrop(item.itemData);
                inventory.RemoveFullStack(item);
            }
        }

        foreach (var equip in equipCopy)
        {
            if(Random.Range(0f,100f) < chanceToLooseItem && equip.HasItem())
            {
                var item = equip.equipedItem;

                CreateItemDrop(item.itemData);
                inventory.UnequipItem(item);
                inventory.RemoveFullStack(item);
            }
        }
    }
}
