using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaExit : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string sceneTransitionName;
    [SerializeField] private bool requiresItem;  // Add this to specify if an item is required for transition
    [SerializeField] private Item.ItemType requiredItemType;  // The required item type for transition
    [SerializeField] private Dialog noItemDialog; // Dialog to show if item is not present

    private float waitToLoadTime = 1f;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<PlayerController1>()) {
            if (!requiresItem || HasRequiredItem(other.gameObject.GetComponent<PlayerController1>())) {
                SceneManagement.Instance.SetTransitionName(sceneTransitionName);
                UIFade.Instance.FadeToBlack();
                StartCoroutine(LoadSceneRoutine());
            } else {
                StartCoroutine(DialogueManager1.Instance.ShowDialog(noItemDialog));
            }
        }
    }

    private bool HasRequiredItem(PlayerController1 playerController) {
        List<Item> playerItems = playerController.inventory.GetItems();
        foreach (Item item in playerItems) {
            if (item.itemType == requiredItemType) {
                return true;
            }
        }
        return false;
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
