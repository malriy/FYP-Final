using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class QuestGiver : NPCController
{
    public bool assignedQuest;
    public bool helped;

    [SerializeField] private GameObject quests;
    [SerializeField] private string questType;
    [SerializeField] private DialogLine[] incompleteDialog;
    [SerializeField] private DialogLine[] completeDialog;
    [SerializeField] private List<QuestItem> itemsToGive;
    [SerializeField] private List<RewardItems> rewards;

    private bool rewarded = false;

    private Quest quest;

    private void Start()
    {

    }

    public override void Interact()
    {
        base.Interact();

        if (!assignedQuest && !helped)
        {
            AssignQuest();
            Debug.Log("Quest assigned");
            
        }
        else if (assignedQuest && !helped)
        {
            dialog.Lines.Clear();
            CheckExistingItems();
            CheckQuestCompletion();
        }
        else
        {
            //dialog.Lines.Clear();
            foreach (DialogLine i in completeDialog)
            {
                dialog.Lines.Add(i);
            }
        }
    }

    void AssignQuest()
    {
        assignedQuest = true;
        quest = (Quest)quests.AddComponent(System.Type.GetType(questType));

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
            //dialog.Lines.Clear();
            RewardPlayer();
            helped = true;
            assignedQuest = false;
            rewarded = true;
        }
        else
        {
            dialog.Lines.Clear();
            foreach (DialogLine i in incompleteDialog)
            {
                dialog.Lines.Add(i);
            }
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

        CheckQuestCompletion();
    }
}
