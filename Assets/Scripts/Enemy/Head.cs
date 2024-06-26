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
        Vector2 direction = (playerPos - (Vector2)rocketPos.position).normalized;

        GameObject shot = Instantiate(rocketPrefab, hole, Quaternion.identity);
        BossProjectile rocket = shot.GetComponent<BossProjectile>();
        rocket.SetDirection(direction);
    }
}
