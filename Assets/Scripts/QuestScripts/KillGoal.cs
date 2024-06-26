using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillGoal : Goal
{
    public int enemyID {  get; set; }
    
    public KillGoal(Quest quest , int enemyID, string description, bool completed, int curAmount, int requiredAmount)
    {
        this.Quest = quest;
        this.enemyID = enemyID;
        this.Description = description;
        this.Completed = completed;
        this.CurrentAmount = curAmount;
        this.RequiredAmount = requiredAmount;  
    }

    public override void Init()
    {
        base.Init();
        Enemy.OnEnemyDied += EnemyDied;
    }

    void EnemyDied(Enemy enemy)
    {
        if (enemy.ID == this.enemyID)
        {
            this.CurrentAmount++;
            Evaluate();
        }
    }
    ~KillGoal()
    {
        Enemy.OnEnemyDied -= EnemyDied;
    }
}
