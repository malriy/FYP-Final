using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    public List<Quest> allQuests;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveQuestState()
    {
        foreach (var quest in allQuests)
        {
            PlayerPrefs.SetInt(quest.questName + "_isCompleted", quest.isCompleted ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    public void LoadQuestState()
    {
        foreach (var quest in allQuests)
        {
            quest.isCompleted = PlayerPrefs.GetInt(quest.questName + "_isCompleted", 0) == 1;
            if (quest.isCompleted)
            {
                quest.SetCompleted();
            }
        }
    }
}

