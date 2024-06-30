using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkJanitor : Quest
{
    // Start is called before the first frame update
    void Start()
    {
        EnochStart.ActivatePark();
        questName = "Park Janitor";
        description = "Kill the drones polluting the park";


        Goals = new List<Goal>
        {
            new KillGoal(this, 0, description, false, 0, 4)
        };

        Goals.ForEach(g => g.Init());
    }

    public override void PostQuest()
    {
        Crowd.DestroyCrowd(1);
    }
}
