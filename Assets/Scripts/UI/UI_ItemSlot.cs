// using Microsoft.Unity.VisualStudio.Editor;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public InventoryItem itemInSlot{get;private set;}
    protected InventoryPlayer inventory;
    protected UI ui;
    protected RectTransform rect;


    [Header("UI Slot Setup")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemStackSize;

    private void Awake()
    {
        inventory = FindAnyObjectByType<InventoryPlayer>();
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if(itemInSlot == null)
            return;
        
        inventory.TryEquipItem(itemInSlot);

        if(itemInSlot == null)
            ui.itemToolTip.showToolTip(false,null);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if(itemInSlot == null) return;
        ui.itemToolTip.showToolTip(true,rect,itemInSlot);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if(itemInSlot == null) return;
        ui.itemToolTip.showToolTip(false,rect,itemInSlot);
    }

    public void UpdateSlot(InventoryItem item = null)
    {
        itemInSlot = item;

        if(itemInSlot == null)
        {
            itemStackSize.text = "";
            itemIcon.color = Color.clear;
            return;
        }

        Color color = Color.white;
        color.a =.9f;
        itemIcon.color = color;
        itemIcon.sprite = itemInSlot.itemData.itemIcon;
        itemStackSize.text = itemInSlot.stackSize > 1 ?itemInSlot.stackSize.ToString() : "";
    }
}
