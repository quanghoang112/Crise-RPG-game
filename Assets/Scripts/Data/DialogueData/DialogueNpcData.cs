using System;
using UnityEngine;

[Serializable]
public class DialogueNpcData
{
    public RewardType npcRewardType;
    public QuestDataSO[] quests;
    public EntityDropManager dropManager;

    public DialogueNpcData(RewardType npcRewardType, QuestDataSO[] quests,EntityDropManager dropManager)
    {
        this.npcRewardType = npcRewardType;
        this.quests = quests;
        this.dropManager = dropManager;
    }
}
