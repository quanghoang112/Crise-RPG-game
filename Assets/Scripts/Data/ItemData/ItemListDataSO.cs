using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item list", fileName = "List of items - ")]
public class ItemListDataSO : ScriptableObject
{
    public ItemDataSO[] itemList;

    public ItemDataSO GetItemData(string saveId)
    {
        return itemList.FirstOrDefault(item => item != null && item.saveID == saveId);
    }

#if UNITY_EDITOR
    [ContextMenu("Auto full with all ItemDataSO")]
    public void CollectItemsData()
    {
        //tìm tất cả các file được tạo bằng ScriptableObject này
        string[] guids = AssetDatabase.FindAssets("t:ItemDataSO");

        itemList = guids
            //load mã guid (saveId) lên RAM
            .Select(guid => AssetDatabase.LoadAssetAtPath<ItemDataSO>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(item => item != null)
            .ToArray();

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();

    }
#endif
}
