using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionGoal : Goal
{
    public Item.ItemType requiredItem;

    public CollectionGoal(Quest quest, Item item, string description, bool completed, int currentAmount, int requiredAmount)
    {
        this.Quest = quest;
        this.requiredItem = item.itemType;
        this.Description = description;
        this.Completed = completed;
        this.CurrentAmount = currentAmount;
        this.RequiredAmount = requiredAmount;
    }

    public override void Init()
    {
        base.Init();
        
        InventoryController.OnItemAdded += ItemPickedUp;
    }

    void ItemPickedUp(object sender, ItemEventArgs e)
    {
        if (e.Item.itemType == this.requiredItem)
        {
            this.CurrentAmount++;
            Evaluate();
        }
    }

    private void OnDestroy()
    {
        InventoryController.OnItemAdded -= ItemPickedUp;
    }
}
