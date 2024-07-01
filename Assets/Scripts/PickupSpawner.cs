using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject healthGlobe, staminaGlobe;
    [SerializeField] private bool enableDrops = false; // Add this option in the Inspector

    public void DropItems() {
        if (!enableDrops) {
            return; // Do nothing if drops are disabled
        }

        int randomNum = Random.Range(0, 4);  // Adjusted range to include 0 for no spawn

        switch (randomNum) {
            case 1:
                Instantiate(healthGlobe, transform.position, Quaternion.identity);
                break;
            case 2:
                Instantiate(staminaGlobe, transform.position, Quaternion.identity);
                break;
            case 3:
                // No item will be spawned, represents the chance to spawn nothing
                break;
            default:
                // This case will also result in no item being spawned
                break;
        }
    }
}
