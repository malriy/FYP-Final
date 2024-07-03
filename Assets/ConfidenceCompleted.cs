using UnityEngine;

public class ConfidenceCompletionHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Assuming the player has the tag "Player"
        {
            LobbyTransitionHandler.ConfidenceCompleted = true;
            Debug.Log("ConfidenceCompleted is now true");
        }
    }
}
