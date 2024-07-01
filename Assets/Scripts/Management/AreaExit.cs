using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;
    [SerializeField] private bool requiresItems;  // Add this to specify if items are required for transition
    [SerializeField] private List<Item.ItemType> requiredItemTypes;  // The required item types for transition
    [SerializeField] private Dialog noItemDialog; // Dialog to show if items are not present

    private float waitToLoadTime = 1f;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<PlayerController1>()) {
            if (!requiresItems || HasRequiredItems(other.gameObject.GetComponent<PlayerController1>())) {
                SceneManagement.Instance.SetTransitionName(sceneTransitionName);
                UIFade.Instance.FadeToBlack();
                StartCoroutine(LoadSceneRoutine());
            } else {
                StartCoroutine(DialogueManager1.Instance.ShowDialog(noItemDialog));
            }
        }
    }

    private bool HasRequiredItems(PlayerController1 playerController) {
        List<Item> playerItems = playerController.inventory.GetItems();
        int itemCount = 0;
        
        foreach (Item.ItemType requiredItemType in requiredItemTypes) {
            foreach (Item item in playerItems) {
                if (item.itemType == requiredItemType) {
                    itemCount++;
                    break; // Exit the inner loop as soon as we find one required item
                }
            }
        }

        // Check if the player has all required items
        return itemCount >= requiredItemTypes.Count;
    }

    private IEnumerator LoadSceneRoutine(){
        while (waitToLoadTime >= 0)
        {
            waitToLoadTime -= Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}
