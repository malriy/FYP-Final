using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBuilding : MonoBehaviour, Interactable
{
    public PlayerController player;
    public Vector2 newPos;

    public void Interact()
    {
        Vector2 pos = new Vector2(newPos.x, newPos.y);

        player.transform.position = pos;
    }
}
