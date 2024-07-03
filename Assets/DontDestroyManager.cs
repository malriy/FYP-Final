using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyTransitionHandler : MonoBehaviour
{
    public string lobbySceneName = "LobbyScene";
    public static bool ConfidenceCompleted = false;

    [SerializeField] private bool requiresItems;  // Add this to specify if items are required for transition
    [SerializeField] private List<Item.ItemType> requiredItemTypes;  // The required item types for transition
    [SerializeField] private Dialog noItemDialog; // Dialog to show if items are not present

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController1 playerController = collision.gameObject.GetComponent<PlayerController1>();
        if (playerController != null)
        {
            if (!requiresItems || HasRequiredItems(playerController))
            {
                ConfidenceCompleted = true;
                StartNPC.lastLevelCompleted = 1;

                LoadLobbyScene();
            }
            else
            {
                StartCoroutine(DialogueManager1.Instance.ShowDialog(noItemDialog));
            }
        }
    }

    private bool HasRequiredItems(PlayerController1 playerController)
    {
        List<Item> playerItems = playerController.inventory.GetItems();
        int itemCount = 0;

        foreach (Item.ItemType requiredItemType in requiredItemTypes)
        {
            foreach (Item item in playerItems)
            {
                if (item.itemType == requiredItemType)
                {
                    itemCount++;
                    break; // Exit the inner loop as soon as we find one required item
                }
            }
        }

        // Check if the player has all required items
        return itemCount >= requiredItemTypes.Count;
    }

    private void LoadLobbyScene()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(lobbySceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == lobbySceneName)
        {
            DestroyObjectsInLobby();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void DestroyObjectsInLobby()
    {
        // List of objects to destroy when in the lobby scene
        string[] objectsToDestroy = { "UICanvas", "GameController", "Player", "DontDestroyManager", "Managers", "Debug Updater" };

        foreach (string objectName in objectsToDestroy)
        {
            GameObject obj = GameObject.Find(objectName);
            if (obj != null)
            {
                Destroy(obj);
            }
        }
    }
}
