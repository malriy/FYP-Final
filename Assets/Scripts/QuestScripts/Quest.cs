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
    public static bool finished;

    public event Action OnQuestCompleted;

    public void CheckGoals()
    {
        isCompleted = Goals.All(g => g.Completed);
        finished = isCompleted;
        if (isCompleted)
        {
            OnQuestCompleted?.Invoke();
        }
    }
}
