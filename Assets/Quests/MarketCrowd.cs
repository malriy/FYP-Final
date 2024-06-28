using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MarketCrowd : Quest
{
    void Start()
    {
        questName = "Merchantising";
        description = "Find a way to disperse the crowd at the market";

        Goals = new List<Goal>
        {
            new CollectionGoal(this, new Item { itemType = Item.ItemType.Lantern }, "", false, 0, 1)
        };

        Goals.ForEach(g => g.Init());
    }

    public override void PostQuest()
    {
        Crowd.DestroyCrowd(0);
    }
}
