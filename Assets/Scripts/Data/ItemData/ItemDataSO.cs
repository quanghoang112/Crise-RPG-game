using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Material Item", fileName = "Material data - ")]
public class ItemDataSO : ScriptableObject
{
    public string saveID{get; private set;}

    [Header("Merchant details")]
    [Range(0,100000)]
    public int itemPrice = 100;
    public int minStackSizeAtShop = 1;
    public int maxStackSizeAtShop = 1;

    [Header("Drop details")]
    [Range(0f,1000f)]
    public int itemRarity = 100;
    [Range(0f,100f)]
    public float dropChance;
    [Range(0f,100f)]
    public float maxDropChance=65f;

    [Header("")]
    public string itemName;
    public Sprite itemIcon;
    public ItemType itemType;
    public int maxStackSize = 1;

    [Header ("Item effect")]
    public ItemEffectDataSO itemEffect;

    [Header("Craft details")]
    public InventoryItem[] craftRecipe;

    private void OnValidate()
    {
        dropChance = GetDropChance();

#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        saveID = AssetDatabase.AssetPathToGUID(path); // nếu nhiều vật phẩm cùng 1 itemDataSO thì có cùng saveID, nhưng nếu 2 itemDataSO khác nhau thì sẽ có saveID khác nhau
#endif
    }

    public float GetDropChance()
    {
        float maxRarity = 1000;
        float chance = (maxRarity - itemRarity + 1)/maxRarity*100;

        return Mathf.Min(chance,maxDropChance);
    }
    
}
