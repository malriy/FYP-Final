using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowd : MonoBehaviour
{
    public int crowdID;
    [SerializeField] private Dialog dialog;
    [SerializeField] private Player player;

    private bool hasInteracted = false; 

    public static void DestroyCrowd(int crowdID)
    {
        Crowd[] allCrowds = FindObjectsOfType<Crowd>();
        foreach (Crowd crowd in allCrowds)
        {
            if (crowd.crowdID == crowdID)
            {
                Destroy(crowd.gameObject);
                return;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasInteracted && collision.CompareTag("Player")) // Check the flag
        {
            StartCoroutine(DialogueManager.Instance.ShowDialog(dialog));
            hasInteracted = true; // Set the flag

            Vector3 direction = (player.transform.position - collision.transform.position).normalized;

            // Move the player a few units away from the crowd
            float distance = 5f; // Adjust the distance as needed
            Vector2 newPosition = player.transform.position + (direction * distance);

            // Set the player's position
            player.transform.position = newPosition;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        hasInteracted = false;
    }
}
