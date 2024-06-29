using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    public GameObject rocketPrefab;
    public Transform rocketPos;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FireRocket()
    {
        Vector3 hole = rocketPos.position;
        Vector2 playerPos = player.position;
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();

        // Predict the future position of the player
        Vector2 playerVelocity = playerRb.velocity;
        float timeToTarget = Vector2.Distance(playerPos, (Vector2)rocketPos.position) / BossProjectile.Speed; 
        Vector2 futurePosition = playerPos + playerVelocity * timeToTarget;

        // Calculate the direction to the future position
        Vector2 direction = (futurePosition - (Vector2)rocketPos.position).normalized;

        // Instantiate and launch the rocket
        GameObject shot = Instantiate(rocketPrefab, hole, Quaternion.identity);
        BossProjectile rocket = shot.GetComponent<BossProjectile>();
        rocket.SetDirection(direction);
    }
}
