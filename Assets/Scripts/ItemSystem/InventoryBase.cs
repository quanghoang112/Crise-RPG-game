using System.Collections.Generic;
using UnityEngine;

public class InventoryBase : MonoBehaviour
{
    public int maxInventorySize = 10;
    public List<InventoryItem> itemList = new List<InventoryItem>();

    public bool CanAddItem() => itemList.Count < maxInventorySize;

    public void AddItem(InventoryItem itemToAdd)
    {
        itemList.Add(itemToAdd);
    }
}
