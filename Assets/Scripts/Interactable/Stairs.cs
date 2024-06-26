using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    public PlayerController player;
    public Vector2 newPos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 pos = new Vector2(newPos.x, newPos.y);

            player.transform.position = pos;
        }
    }
}
