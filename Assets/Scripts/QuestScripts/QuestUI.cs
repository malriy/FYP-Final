using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private Transform questContainer;
    [SerializeField] private Transform questSingle;


    private void Start()
    {
        
    }

    private void Update()
    {
        DisplayActiveQuests();
    }

    private void DisplayActiveQuests()
    {
        foreach (Transform child in questContainer)
        {
            if (child == questSingle) continue;
            Destroy(child.gameObject);
        }

        // Find all active quests
        Quest[] activeQuests = FindObjectsOfType<Quest>();

        // Build the quest display string
        //string questDisplay = "Active Quests:\n";
        foreach (Quest quest in activeQuests)
        {
            if (!quest.isCompleted)
            {
                RectTransform invSlotRectTransform = Instantiate(questSingle, questContainer).GetComponent<RectTransform>();
                invSlotRectTransform.gameObject.SetActive(true);

                TextMeshProUGUI nameText = invSlotRectTransform.Find("QuestName").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI descText = invSlotRectTransform.Find("QuestDesc").GetComponent<TextMeshProUGUI>();


                nameText.text = quest.questName;
                descText.text = quest.description;
            }
        }
    }
}
