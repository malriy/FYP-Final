using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneKiller : Quest
{
    // Start is called before the first frame update
    void Start()
    {
        questName = "Drone Killer";
        description = "Save the worried father's son";

        Goals = new List<Goal>
        {
            new KillGoal(this, 0, description, false, 0, 4)
        };

        Goals.ForEach(g => g.Init());
    }
}