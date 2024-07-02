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
    public event Action OnQuestCompleted;

    public void CheckGoals()
    {
        isCompleted = Goals.All(g => g.Completed);
        if (isCompleted)
        {
            OnQuestCompleted?.Invoke();
        }
    }

    public virtual void PostQuest()
    {

    }

    public void SetCompleted()
    {
        isCompleted = true;
        OnQuestCompleted?.Invoke();
    }
}
