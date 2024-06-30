using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Quest : MonoBehaviour
{
    public List<Goal> Goals { get; set; } = new List<Goal>();
    public string questName;
    public string description;
    public bool isCompleted;

    public static Dictionary<string, bool> questCompletionStatus = new Dictionary<string, bool>();
    public event Action OnQuestCompleted;

    private void Start()
    {
        // Initialize the quest completion status in the dictionary
        questCompletionStatus[questName] = false;
    }

    public void CheckGoals()
    {
        isCompleted = Goals.All(g => g.Completed);
        if (isCompleted)
        {
            questCompletionStatus[questName] = true;
            OnQuestCompleted?.Invoke();
        }
    }

    public virtual void PostQuest()
    {

    }

    public static bool IsQuestFinished(string questName)
    {
        return questCompletionStatus.ContainsKey(questName) && questCompletionStatus[questName];
    }
}
