// using System.Text;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemInfo;

    [SerializeField] private TextMeshProUGUI itemPrice;
    [SerializeField] private Transform merchantInfo;

    public void showToolTip(bool show, RectTransform targetRect, InventoryItem itemToShow, bool buyPrice = false, bool showMerchantInfo = false)
    {
        base.showToolTip(show, targetRect);

        merchantInfo.gameObject.SetActive(showMerchantInfo);

        int price = buyPrice ? itemToShow.buyPrice : Mathf.FloorToInt(itemToShow.sellPrice);
        int totalPrice = price * itemToShow.stackSize;

        string singleStackPrice = $"Price: {price}g.";

        itemName.text = itemToShow.itemData.itemName;
        itemType.text = itemToShow.itemData.itemType.ToString();
        itemPrice.text = singleStackPrice;
        itemInfo.text = itemToShow.GetItemInfo();

        string color = GetColorByRarity(itemToShow.itemData.itemRarity);
        itemName.text = GetColoredText(color,itemToShow.itemData.itemName);

    }

    private string GetColorByRarity (int rarity)
    {
        if(rarity <= 100)   return "white";
        if(rarity <= 300)   return "green";
        if(rarity <= 600)   return "blue";
        if(rarity <= 800)   return "purple";
        return "orange";
        
    }    
    
    
}
