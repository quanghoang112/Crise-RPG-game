using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Material Item", fileName = "Material data - ")]
public class ItemDataSo : ScriptableObject
{
    [Header("")]
    public string itemName;
    public Sprite itemIcon;
    public ItemType itemType;
}
