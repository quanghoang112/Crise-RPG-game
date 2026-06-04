using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryBase : MonoBehaviour
{
    public event Action OnInventoryChange;
    public int maxInventorySize = 10;
    public List<InventoryItem> itemList = new List<InventoryItem>();


    protected virtual void Awake()
    {
        
    }

    public void TryUseItem(InventoryItem itemToUse)
    {
        InventoryItem consumable = itemList.Find(item => item.itemId == itemToUse.itemId);

        if(consumable == null)
        {
            // Debug.Log("null");
            return;
        }
        
        consumable.itemEffect.ExecuteEffect();

        if(consumable.stackSize > 1)
            consumable.RemoveStack();
        else
            RemoveOneItem(consumable);

        OnInventoryChange?.Invoke();
    }
    
    public bool CanAddItem(InventoryItem itemToAdd) 
    {
        bool hasStackable = FindStackable(itemToAdd) != null;

        return hasStackable || itemList.Count < maxInventorySize;
    }

    public InventoryItem FindStackable(InventoryItem itemToAdd)
    {
        List<InventoryItem> stackableItems = itemList.FindAll(item => item.itemData == itemToAdd.itemData);

        foreach (var stackableItem in stackableItems)
        {
            if(stackableItem.CanAddStack())
                return stackableItem;
        }
        return null;
    }
    
    public void OnTriggerUpdateUI() => OnInventoryChange?.Invoke();

    public void EquipItem(InventoryItem item)
    {
        // item.AddModifiers();
    }

    public void AddItem(InventoryItem itemToAdd)
    {
        // itemList.Add(itemToAdd);

        InventoryItem itemInInventory = FindStackable(itemToAdd);
        // nếu dùng FindItem sẽ tìm được thằng đầu tiên đã max stack trong khi các thằng sau chưa max
        if(itemInInventory != null)
            itemInInventory.AddStack();
        else
            itemList.Add(itemToAdd);

        OnInventoryChange?.Invoke();
    }

    public void RemoveOneItem(InventoryItem itemToRemove)
    {
        InventoryItem itemInInventory = itemList.Find(item => item == itemToRemove);

        if(itemInInventory.stackSize > 1)
            itemInInventory.RemoveStack();
        else
            itemList.Remove(itemToRemove);

        OnInventoryChange?.Invoke();
    }

    public InventoryItem FindItem(ItemDataSO itemData)
    {
        return itemList.Find(item => item.itemData == itemData);
    }
}
