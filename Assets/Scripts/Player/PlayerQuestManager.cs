// #nullable enable

using System.Collections.Generic;
using UnityEngine;

public class PlayerQuestManager : MonoBehaviour, ISaveable
{
    public List<QuestData> activeQuests;
    public List<QuestData> completedQuests;

    private InventoryPlayer inventory;

    [SerializeField] private QuestDataBaseSO questDB;
    // private EntityDropManager dropManager;

    private void Awake()
    {
        inventory=GetComponent<InventoryPlayer>();
    }



    private void GiveQuestReward(QuestDataSO questDataSO,EntityDropManager dropManager = null)
    {
        foreach(var item in questDataSO.rewardItems)
        {
            if(item == null || item.itemData == null)   continue;

            for(int i =0;i<item.stackSize; i++)
            {
                dropManager?.CreateItemDrop(item.itemData);
            }
        }
    }

    public void TryGiveRewardFrom(RewardType npcType, EntityDropManager dropManager = null)
    {
        List<QuestData> getRewardQuests = new List<QuestData>();

        foreach(var quest in activeQuests)
        {
            //Deliver items if can
            if(quest.questDataSO.questType == QuestType.Delivery)
            {
                var requiredItem = quest.questDataSO.itemToDeliver;
                var requiredAmount = quest.questDataSO.requiredAmount;
                // Debug.Log(inventory.HasItemAmount(requiredItem,requiredAmount));
                if(inventory.HasItemAmount(requiredItem,requiredAmount) == true)
                {
                    inventory.RemoveItemAmount(requiredItem,requiredAmount);
                    quest.AddQuestProgress(requiredAmount);
                }
            
            if(quest.CanGetReward() && quest.questDataSO.rewardType == npcType)
                getRewardQuests.Add(quest);
            }
        }

        foreach(var quest in getRewardQuests)
        {
            GiveQuestReward(quest.questDataSO,dropManager);
            CompleteQuest(quest);
        }


    }

    public void AddProgress(string questTargetId, int amount = 1, EntityDropManager dropManager = null)
    {
        List<QuestData> getRewardQuests = new List<QuestData>();
        foreach(var quest in activeQuests)
        {
            if(quest.questDataSO.questTargetId != questTargetId) continue;


            if(quest.CanGetReward() == false)
            {
                quest.AddQuestProgress(amount);
            }

            if(quest.questDataSO.rewardType == RewardType.None && quest.CanGetReward())
            {
                getRewardQuests.Add(quest);
            }
        }

        foreach(var quest in getRewardQuests)
        {
            GiveQuestReward(quest.questDataSO, dropManager);
            CompleteQuest(quest);
        }
    }


    public void AcceptQuest(QuestDataSO questDataSO)
    {
        activeQuests.Add(new QuestData(questDataSO));
    }

    public void CompleteQuest(QuestData questData)
    {
        completedQuests.Add(questData);
        activeQuests.Remove(questData);
    }

    public bool QuestIsActive(QuestDataSO questToCheck)
    {
        if(questToCheck == null)
            return false;

        return activeQuests.Find(q => q.questDataSO == questToCheck) != null;
        
    }

    public bool QuestIsCompleted(QuestDataSO questToCheck)
    {
        if(questToCheck == null)
            return false;

        return completedQuests.Find(q => q.questDataSO == questToCheck) != null;
    }

    public void LoadData(GameData data)
    {
        activeQuests.Clear();
        completedQuests.Clear();


        foreach (var entry in data.activeQuests)
        {
            string questSaveId = entry.Key;
            int progress = entry.Value;

            QuestDataSO questDataSO = questDB.GetQuestById(questSaveId);

            if(questDataSO == null)
            {
                continue;
            }

            QuestData questToLoad = new QuestData(questDataSO);
            questToLoad.currentAmount = progress;

            activeQuests.Add(questToLoad);
        }

        foreach (var entry in data.completedQuests)
        {
            string questSaveId = entry.Key;

            QuestDataSO questDataSO = questDB.GetQuestById(questSaveId);

            if(questDataSO == null)
            {
                continue;
            }

            QuestData questToLoad = new QuestData(questDataSO);

            completedQuests.Add(questToLoad);

        }
    }

    public void SaveData(ref GameData data)
    {
        data.activeQuests.Clear();
        data.completedQuests.Clear();

        foreach(var quest in activeQuests)
        {
            data.activeQuests.Add(quest.questDataSO.questSaveId, quest.currentAmount);
        }

        foreach(var quest in completedQuests)
        {
            data.completedQuests.Add(quest.questDataSO.questSaveId, true);
        }
    }
}
