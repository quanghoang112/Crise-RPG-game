using System.Linq;
using UnityEditor;
using UnityEngine;



[CreateAssetMenu(menuName = "RPG Setup/Quest Data/ Quest Database", fileName = "QUEST DATABASE")]
public class QuestDataBaseSO : ScriptableObject
{
    public QuestDataSO[] allQuests;

    public QuestDataSO GetQuestById(string id)
    {
        return allQuests.FirstOrDefault(q => q.questSaveId == id);
    }

    #if UNITY_EDITOR
    [ContextMenu("Auto-fill with all QuestDataSO")]
    public void CollectItemsData()
    {
        //tìm tất cả các file được tạo bằng ScriptableObject này
        string[] guids = AssetDatabase.FindAssets("t:QuestDataSO");

        allQuests = guids
            //load mã guid (saveId) lên RAM
            .Select(guid => AssetDatabase.LoadAssetAtPath<QuestDataSO>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(q => q != null)
            .ToArray();

        EditorUtility.SetDirty(this);//tự động lưu trên hệ thống.
        AssetDatabase.SaveAssets();

    }

    #endif

}
