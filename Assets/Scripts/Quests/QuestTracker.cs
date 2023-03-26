using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Class))]
public class QuestTracker : MonoBehaviour
{
    public static event Action<Quest> OnCollectQuest;

    [SerializeField] QuestGenerator _questGenerator;
    
    public int Score { get; private set; } = 0;
    public int TotalQuestCount { get; private set; } = 0;
    public Dictionary<Quest.QuestType, List<Quest>> CollectedQuests = new Dictionary<Quest.QuestType, List<Quest>>();

    Class _class;

    void Start()
    {
        _class = GetComponent<Class>();

        // collect a quest with no value of each type
        // this will ensure the quest generator works as intended even before the player completes any quests
        foreach (Quest.QuestType type in Enum.GetValues(typeof(Quest.QuestType)))
        {
            // ensure the internal quest type Quest.QuestType.None does not have a quest added
            if (type != Quest.QuestType.None)
            {
                CollectQuest(new Quest(type, 0), true);
            }
        }
    }
    
    public void CollectQuest(Quest quest, bool isInitializerQuest = false)
    {
        // if the list of collected quests doesn't have any quests of the new quest's type, add a new entry to contain quests of that type
        if (!CollectedQuests.ContainsKey(quest.Type))
        {
            CollectedQuests.Add(quest.Type, new List<Quest>());
        }

        // add the quest to the list of quests with the same type
        CollectedQuests[quest.Type].Add(quest);

        // increment score and total quest count
        Score += CalculateScore(quest);
        TotalQuestCount++;

        if (!isInitializerQuest)
        {
            OnCollectQuest?.Invoke(quest);
        }
    }

    public int CalculateScore(Quest quest)
    {
        if (quest.Type == _class.BonusQuestType)
        {
            // return the value multiplied by the bonus modifier
            return (int)(quest.Value * Class.CLASS_BONUS_MODIFIER);
        }
        else
        {
            // return the regular value
            return quest.Value;
        }
    }
}