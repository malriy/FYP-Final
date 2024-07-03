using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class QuestGiver : NPCController
{
    [NonSerialized] public bool assignedQuest;
    [NonSerialized] public bool helped;

    private GameObject quests;
    [SerializeField] private string questName;
    [SerializeField] private Dialog incompleteDialog;
    [SerializeField] private Dialog completeDialog;
    [SerializeField] private List<QuestItem> itemsToGive;
    [SerializeField] private List<RewardItems> rewards;

    private bool rewarded = false;

    private Quest quest;

    private void Start()
    {
        quests = GameObject.Find("Quests");
    }

    public override void Interact()
    {
        if (!assignedQuest && !helped)
        {
            base.Interact();
            AssignQuest();
            hasTalked = false;
        }
        else if (assignedQuest && !helped)
        {
            CheckExistingItems();
            CheckQuestCompletion();
            hasTalked = false;
        }
        else if (assignedQuest && helped && !hasTalked)
        {
            StartCoroutine(DialogueManager.Instance.ShowDialog(completeDialog));
            hasTalked = true;
        }
        else { base.Interact(); }
    }

    void AssignQuest()
    {
        assignedQuest = true;
        quest = (Quest)quests.AddComponent(System.Type.GetType(questName));

        foreach (QuestItem questItem in itemsToGive)
        {
            Item item = new Item { itemType = questItem.itemType, amount = questItem.amount };
            if (player != null)
            {
                player.inventory.AddItem(item);
            }
            if (player1 != null)
            {
                player1.inventory.AddItem(item);
            }
        } 
    }

    void RewardPlayer()
    {
        if (!rewarded)
        {
            foreach (RewardItems rewardItems in rewards)
            {
                Item item = new Item { itemType = rewardItems.itemType, amount = rewardItems.amount };
                if (player != null)
                {
                    player.inventory.AddItem(item);
                }
                if (player1 != null)
                {
                    player1.inventory.AddItem(item);
                }
            }
        }
    }

    void CheckQuestCompletion()
    {
        if (quest.isCompleted)
        {
            RewardPlayer();
            DeleteQuestItems();
            helped = true;
            assignedQuest = true;
            rewarded = true;

            quest.PostQuest();
        }
        else
        {
            StartCoroutine(DialogueManager.Instance.ShowDialog(incompleteDialog));
        }
    }

    void CheckExistingItems()
    {
        CollectionGoal collectionGoal = quest.Goals.Find(goal => goal is CollectionGoal) as CollectionGoal;
        if (collectionGoal != null)
        {
            if (player != null)
            {
                List<Item> items = player.inventory.GetItems();
                foreach (Item item in items)
                {
                    if (item.itemType == collectionGoal.requiredItem)
                    {
                        collectionGoal.CurrentAmount += item.amount;
                        collectionGoal.Evaluate();
                    }
                }
            }
            if (player1 != null)
            {
                List<Item> items = player1.inventory.GetItems();
                foreach (Item item in items)
                {
                    if (item.itemType == collectionGoal.requiredItem)
                    {
                        collectionGoal.CurrentAmount += item.amount;
                        collectionGoal.Evaluate();
                    }
                }
            }
            
        }
    }

    void DeleteQuestItems()
    {
        CollectionGoal collectionGoal = quest.Goals.Find(goal => goal is CollectionGoal) as CollectionGoal;
        if (collectionGoal != null)
        {
            var playerInstance = player != null ? player.inventory : PlayerController1.Instance.inventory;
            playerInstance.RemoveItem(new Item { itemType = collectionGoal.requiredItem, amount = collectionGoal.RequiredAmount });
        }
    }
}
