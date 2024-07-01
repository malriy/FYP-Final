using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour, IEnemy
{
    [SerializeField] private float attackRadius = 5f;
    [SerializeField] private float roamRadius = 10f;

    private Vector2 roamPosition;
    private EnemyPathfinding enemyPathfinding;

    private void Start() {
        roamPosition = GetRandomRoamPosition();
        enemyPathfinding = GetComponent<EnemyPathfinding>();
    }

    private void Update() {
        if (Vector2.Distance(transform.position, PlayerController1.Instance.transform.position) <= attackRadius)
        {
            // Attack the player
            Vector2 targetDirection = PlayerController1.Instance.transform.position - transform.position;
            enemyPathfinding.MoveTo(targetDirection.normalized); // Use EnemyPathfinding for movement
        }
        else
        {
            // Roam around
            if (Vector2.Distance(transform.position, roamPosition) < 1f)
            {
                roamPosition = GetRandomRoamPosition();
            }
            else
            {
                Vector2 roamDirection = roamPosition - (Vector2)transform.position;
                enemyPathfinding.MoveTo(roamDirection.normalized); // Use EnemyPathfinding for movement
            }
        }
    }

    private Vector2 GetRandomRoamPosition() {
        return (Vector2)transform.position + Random.insideUnitCircle * roamRadius;
    }

    public void Attack() {
        // Implement attack logic here
    }
}
