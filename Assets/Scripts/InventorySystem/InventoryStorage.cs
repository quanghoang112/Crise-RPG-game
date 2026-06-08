using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryStorage : InventoryBase
{
    public InventoryPlayer playerInventory{get; private set;}
    public List<InventoryItem> materialStash;// material item lưu trữ trong storage



    public void CraftItem(InventoryItem itemToCraft)
    {
        ConsumeMaterials(itemToCraft);
        playerInventory.AddItem(itemToCraft);
    }

    public bool CanCraftItem(InventoryItem itemToCraft)
    {
        return HasEnoughMaterials(itemToCraft) && playerInventory.CanAddItem(itemToCraft);
    }

// trừ trong túi player trc rồi mới trừ trong storage
    private void ConsumeMaterials(InventoryItem itemToCraft) 
    {
        foreach(var requiredItem in itemToCraft.itemData.craftRecipe)
        {
            int amountToConsume = requiredItem.stackSize;
            amountToConsume = amountToConsume - ConsumedMaterialsAmount(playerInventory.itemList,requiredItem);

            if(amountToConsume > 0)
                amountToConsume = amountToConsume - ConsumedMaterialsAmount(itemList,requiredItem);

            if(amountToConsume > 0)
                amountToConsume = amountToConsume - ConsumedMaterialsAmount(materialStash, requiredItem);
        }
    }

    private int ConsumedMaterialsAmount(List<InventoryItem> itemList, InventoryItem neededItem)
    {
        int amountNeeded = neededItem.stackSize;
        int consumedAmount = 0;
        
        foreach (var item in itemList)
        {
            if(item.itemData != neededItem.itemData)
                continue;
        
            int removeAmount = Mathf.Min(item.stackSize,amountNeeded- consumedAmount);
            item.stackSize = item.stackSize - removeAmount;
            consumedAmount += removeAmount;

            if(item.stackSize<=0)
                itemList.Remove(item);

            if(consumedAmount >=amountNeeded)
                break;
        }

        return consumedAmount;
    }

    public bool HasEnoughMaterials(InventoryItem itemToCraft)
    {
        foreach (var requiredMaterial in itemToCraft.itemData.craftRecipe)
        {
            if(GetAvailableAmountOf(requiredMaterial.itemData) < requiredMaterial.stackSize)
                return false;
        }
        return true;
    }    

    public int GetAvailableAmountOf(ItemDataSO requiredItem)
    {
        int amount = 0;
        foreach(var item in playerInventory.itemList)
        {
            if(item.itemData == requiredItem)
                amount+= item.stackSize;
        }

        foreach(var item in itemList)
        {
            if(item.itemData == requiredItem)
                amount += item.stackSize;
        }

        foreach(var item in materialStash)
        {
            if(item.itemData == requiredItem)
                amount += item.stackSize;
        }

        return amount;
    }

    public void AddMaterialToStash(InventoryItem itemToAdd)
    {
        var stackableItem = StackableInStash(itemToAdd);

        if(stackableItem != null)
            stackableItem.AddStack();
        else
            materialStash.Add(itemToAdd);

        OnTriggerUpdateUI();
        materialStash = materialStash.OrderBy(item => item.itemData.name).ToList();
    }

    public InventoryItem StackableInStash(InventoryItem itemToAdd)
    {
        List<InventoryItem> stackableItems = materialStash.FindAll (item => item.itemData == itemToAdd.itemData);

        foreach(var stackable in stackableItems)
        {
            if(stackable.CanAddStack())
                return stackable;
        }

        return null;
    }

    public void SetInventory (InventoryPlayer inventory) => this.playerInventory = inventory;

    public void FromPlayerToStorage(InventoryItem item, bool transferFullStack = false)
    {
        int transferAmount = transferFullStack? item.stackSize : 1;

        for(int i = 0;i<transferAmount;i++)
        {
            if(CanAddItem(item))
            {
                var itemToAdd = new InventoryItem(item.itemData);
                playerInventory.RemoveOneItem(item);
                AddItem(itemToAdd);
            }
        }

        OnTriggerUpdateUI();
    }

    public void FromStorageToPlayer(InventoryItem item, bool transferFullStack = false)
    {
        int transferAmount = transferFullStack? item.stackSize : 1;

        for(int i = 0;i<transferAmount;i++)
        {
            if(playerInventory.CanAddItem(item))
            {
                var itemToAdd = new InventoryItem(item.itemData);

                RemoveOneItem(item);
                playerInventory.AddItem(itemToAdd);
            }
        }

        OnTriggerUpdateUI();
    }

    public override void SaveData(ref GameData data)
    {
        base.SaveData(ref data);

        data.storageItems.Clear();

        foreach (var item in itemList)
        {
            if(item != null && item.itemData != null)
            {
                string saveId = item.itemData.saveID;
                int stack = item.stackSize;

                if(data.storageItems.ContainsKey(saveId) == false)
                    data.storageItems[saveId] = 0;
                    
                data.storageItems[saveId] += item.stackSize;
            }
        }

        data.storageMaterials.Clear();

        foreach (var item in itemList)
        {
            if(item != null && item.itemData != null)
            {
                string saveId = item.itemData.saveID;
                int stack = item.stackSize;

                if(data.storageMaterials.ContainsKey(saveId) == false)
                    data.storageMaterials[saveId] = 0;
                    
                data.storageMaterials[saveId] += item.stackSize;
            }
        }
    }

    public override void LoadData(GameData data)
    {
        base.LoadData(data);

        itemList.Clear();
        materialStash.Clear();

        foreach (var entry in data.storageItems)
        {
            string saveId = entry.Key;
            int stackSize = entry.Value;

            ItemDataSO itemData = itemDataBase.GetItemData(saveId);

            if (itemData == null)
            {
                Debug.LogWarning("Item not found: " + saveId);
                continue;
            }


            for (int i = 0; i < stackSize; i++)
            {
                InventoryItem itemToLoad = new InventoryItem(itemData);
                AddItem(itemToLoad);
            }
        }



        foreach (var entry in data.storageMaterials)
        {
            string saveId = entry.Key;
            int stackSize = entry.Value;

            ItemDataSO itemData = itemDataBase.GetItemData(saveId);

            if (itemData == null)
            {
                Debug.LogWarning("Item not found: " + saveId);
                continue;
            }


            for (int i = 0; i < stackSize; i++)
            {
                InventoryItem itemToLoad = new InventoryItem(itemData);
                AddMaterialToStash(itemToLoad);
            }
        }

        OnTriggerUpdateUI();
    }
}
