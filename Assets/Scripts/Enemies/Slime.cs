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
            Vector2 targetDirection = PlayerController1.Instance.transform.position - transform.position;
            transform.position += enemyPathfinding.moveSpeed * Time.deltaTime * (Vector3)targetDirection.normalized;
        }
        else
        {
            if (Vector2.Distance(transform.position, roamPosition) < 1f)
            {
                roamPosition = GetRandomRoamPosition();
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, roamPosition, enemyPathfinding.moveSpeed * Time.deltaTime);
            }
        }
    }

    private Vector2 GetRandomRoamPosition() {
        return (Vector2)transform.position + Random.insideUnitCircle.normalized * roamRadius;
    }

    public void Attack(){}
}