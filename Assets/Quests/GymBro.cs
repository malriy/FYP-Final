using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GymBro : Quest
{
    // Start is called before the first frame update
    void Start()
    {
        questName = "Gym Bro";
        description = "Get a gym membership card";


        Goals = new List<Goal>
        {
            new CollectionGoal(this, new Item{ itemType = Item.ItemType.GymCard }, description, false,0,1),
            //new CollectionGoal(this, new Item{ itemType = Item.ItemType.ManaPotion }, description, false,0,3)

        };

        Goals.ForEach(g => g.Init());
    }

    public override void PostQuest()
    {
        Crowd.DestroyCrowd(2);
    }
}
