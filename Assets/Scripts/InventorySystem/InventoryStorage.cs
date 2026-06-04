using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryStorage : InventoryBase
{
    public InventoryPlayer playerInventory{get; private set;}
    public List<InventoryItem> materialStash;// material item lưu trữ trong storage


// trừ trong túi player trc rồi mới trừ trong storage
    public void ConsumeMaterials(InventoryItem itemToCraft) 
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
}
