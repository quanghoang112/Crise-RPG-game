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
    
    public bool CanAddItem() => itemList.Count < maxInventorySize;
    
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
    
    // public bool CanAddToStack (InventoryItem itemToAdd)
    // {
    //     List<InventoryItem> stackableItems = itemList.FindAll(item => item.itemData == itemToAdd.itemData);

    //     foreach (var stack in stackableItems)
    //     {
    //         if(stack.CanAddStack())
    //             return true;
    //     }
    //     return false;
    // }

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

    public void RemoveItem(InventoryItem itemToRemove)
    {
        itemList.Remove(FindItem(itemToRemove.itemData));
        OnInventoryChange?.Invoke();
    }

    public InventoryItem FindItem(ItemDataSO itemData)
    {
        return itemList.Find(item => item.itemData == itemData);
    }
}
