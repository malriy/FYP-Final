using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowd : MonoBehaviour
{
    public int crowdID;
    [SerializeField] private Dialog dialog;
    [SerializeField] private Player player;

    private int numofCrowds;
    private bool hasInteracted = false; 

    public static void DestroyCrowd(int crowdID)
    {
        Crowd[] allCrowds = FindObjectsOfType<Crowd>();
        foreach (Crowd crowd in allCrowds)
        {
            if (crowd.crowdID == crowdID)
            {
                Destroy(crowd.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasInteracted && collision.CompareTag("Player")) // Check the flag
        {
            StartCoroutine(DialogueManager.Instance.ShowDialog(dialog));
            hasInteracted = true;

            Crowd[] allCrowds = FindObjectsOfType<Crowd>();
            numofCrowds = allCrowds.Length;
            Debug.Log(numofCrowds);

            Vector3 direction = (player.transform.position - collision.transform.position).normalized;
            player.GetComponent<PlayerHealth1>().TakeDamage(numofCrowds, transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        hasInteracted = false;
    }
}
