using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Material Item", fileName = "Material data - ")]
public class ItemDataSO : ScriptableObject
{
    [Header("")]
    public string itemName;
    public Sprite itemIcon;
    public ItemType itemType;
    public int maxStackSize = 1;

    [Header ("Item effect")]
    public ItemEffectDataSO itemEffect;

    [Header("Craft details")]
    public InventoryItem[] craftRecipe;
    
}
