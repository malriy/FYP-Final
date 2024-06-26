using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToLevel : MonoBehaviour, Interactable
{
    public Vector2 spawnPoint; // Define the spawn point in the Unity Editor
    public PlayerController player;

    public void Interact()
    {
        player.transform.position = spawnPoint;
    }
}

