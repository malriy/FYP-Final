using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCreamKid : Quest
{
    // Start is called before the first frame update
    void Start()
    {
        questName = "Ice Scream";
        description = "Obtain some ice cream";

        Goals = new List<Goal>
        {
            new CollectionGoal(this, new Item{ itemType = Item.ItemType.IceCream }, description, false, 0, 1)
        };

        Goals.ForEach(g => g.Init());
    }
}
