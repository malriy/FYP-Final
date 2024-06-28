using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetBasketball : Quest
{
    // Start is called before the first frame update
    void Start()
    {
        questName = "Get Basketball";
        description = "Obtain a basketball";

        Goals = new List<Goal>
        {
            new CollectionGoal(this, new Item{ itemType = Item.ItemType.Basketball }, description, false, 0, 1)
        };

        Goals.ForEach(g => g.Init());
    }
}
