using System;
using UnityEngine;
[CreateAssetMenu(menuName ="RPG Setup/Item Data/Equipment item", fileName = "Equipment data - ")]
public class EquipmentDataSO : ItemDataSO
{
    [Header("Item modifiers")]
    public ItemModifier[] modifiers;
}


[Serializable]
public class ItemModifier
{
    public StatsType statsType;
    public float value;
}
