using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryBase : MonoBehaviour, ISaveable
{
    protected Player player;
    public event Action OnInventoryChange;
    public int maxInventorySize = 10;
    public List<InventoryItem> itemList = new List<InventoryItem>();

    [Header("Item list base")]
    [SerializeField] protected ItemListDataSO itemDataBase;


    protected virtual void Awake()
    {
        player = GetComponent<Player>();
    }

    public void TryUseItem(InventoryItem itemToUse)
    {
        InventoryItem consumable = itemList.Find(item => item.itemId == itemToUse.itemId);

        if(consumable == null)
        {
            // Debug.Log("null");
            return;
        }

        if(consumable.itemEffect.CanBeUsed(player) == false)  return;
            
        
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

    public void RemoveFullStack(InventoryItem itemToRemove)
    {
        for (int i = 0;i< itemToRemove.stackSize; i++)
        {
            RemoveOneItem(itemToRemove);
        }
    }

    public InventoryItem FindSameItem(InventoryItem itemToFind)
    {
        return itemList.Find(item => item.itemData == itemToFind.itemData);
    }

    public InventoryItem FindItem(InventoryItem itemToFind)
    {
        return itemList.Find(item => item == itemToFind);
    }

    public void RemoveItemAmount(ItemDataSO itemToRemove, int amount)
    {
        for (int i = 0;i<itemList.Count; i++)
        {
            InventoryItem item = itemList[i];

            if(item.itemData != itemToRemove)
                continue;

            int removeCount = Mathf.Min(amount, item.stackSize);

            for(int j=0; j<removeCount; j++)
            {
                RemoveOneItem(item);
                amount--;
                if(amount <=0)
                    break;
            }
        }
    }

    public bool HasItemAmount(ItemDataSO itemToCheck, int amount)
    {
        int total = 0;
        foreach(var item in itemList)
        {
            if(item.itemData == itemToCheck)
                total += item.stackSize;

            if(total >= amount)
                return true;
        }

        return false;
    }

    public virtual void LoadData(GameData data)
    {
        
    }

    public virtual void SaveData(ref GameData data)
    {
        
    }
}
