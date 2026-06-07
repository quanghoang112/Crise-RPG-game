using System.Collections.Generic;
using UnityEngine;

public class InventoryMerchant : InventoryBase
{
    private InventoryPlayer inventory;

    [SerializeField] private ItemListDataSO shopData;
    [SerializeField] private int minItemsAmount = 4;


    protected override void Awake()
    {
        base.Awake();
        FillShopList();
    }

    public void TryBuyItem(InventoryItem itemToBuy, bool buyFullStack)
    {
        int amountToBuy = buyFullStack ? itemToBuy.stackSize : 1;

        for(int i = 0; i< amountToBuy; i++)
        {
            if(inventory.gold < itemToBuy.buyPrice)
            {
                Debug.Log("Not enough money!");
                return;
            }

            var itemToAdd = new InventoryItem(itemToBuy.itemData);
            if(itemToBuy.itemData.itemType == ItemType.Material)
            {
                inventory.storage.AddMaterialToStash(itemToAdd);
            }
            else
            {
                if(inventory.CanAddItem(itemToBuy))
                {
                    inventory.AddItem(itemToAdd);
                }

            }
            inventory.gold -= itemToBuy.buyPrice;
            RemoveOneItem(itemToBuy);

        }

        inventory.OnTriggerUpdateUI();
    }

    public void TrySellItem(InventoryItem itemToSell, bool sellFullStack)
    {
        int amountToSell = sellFullStack ? itemToSell.stackSize : 1;

        for (int i = 0; i < amountToSell ; i++)
        {
            int sellPrice = Mathf.FloorToInt(itemToSell.sellPrice);

            inventory.gold = inventory.gold + sellPrice;
            inventory.RemoveOneItem(itemToSell);
        }

        inventory.OnTriggerUpdateUI();
    }

    public void FillShopList()
    {
        itemList.Clear();
        List<InventoryItem> possibleItems = new List<InventoryItem>();

        foreach (var itemData in shopData.itemList)
        {
            int randomizedStack = Random.Range(itemData.minStackSizeAtShop, itemData.maxStackSizeAtShop +1);
            int finalStack = Mathf.Clamp(randomizedStack,1, itemData.maxStackSize);

            InventoryItem itemToAdd = new InventoryItem(itemData);
            itemToAdd.stackSize = finalStack;
            // itemToAdd.stackSize = 2;

            possibleItems.Add(itemToAdd);
        }

        int randomItemAmount = Random.Range(minItemsAmount, maxInventorySize + 1);
        int finalAmount = Mathf.Clamp(randomItemAmount,1, possibleItems.Count);

        for (int i=0;i< finalAmount;i++)
        {
            var randomIndex = Random.Range(0,possibleItems.Count);
            var item = possibleItems[randomIndex];

            if(CanAddItem(item))
            {
                possibleItems.Remove(item);
                AddItem(item);
            }
        }

        OnTriggerUpdateUI();
    }

    public void SetInventory(InventoryPlayer inventory) => this.inventory = inventory;
}
