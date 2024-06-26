using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneKiller : Quest
{
    // Start is called before the first frame update
    void Start()
    {
        questName = "Drone Killer";
        description = "Kill those two drones";

        Goals = new List<Goal>
        {
            new KillGoal(this, 0, "Kill the two drones.", false, 0, 4)
        };

        Goals.ForEach(g => g.Init()); 
    }
}