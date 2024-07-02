using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnochMain : Quest
{
    void Start()
    {
        questName = "Enochlophobia";
        description = "Find a way into the gun store";

        Goals = new List<Goal>
        {
            new CollectionGoal(this, new Item { itemType = Item.ItemType.GunStoreKey }, "", false, 0, 1)
        };

        Goals.ForEach(g => g.Init());
    }

    public override void PostQuest()
    {
        GunStore.locked = false;
    }

}
