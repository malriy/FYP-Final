using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyTransitionHandler : MonoBehaviour
{
    public string lobbySceneName = "LobbyScene";
    public static bool ConfidenceCompleted = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        ConfidenceCompleted = true;
        LoadLobbyScene();
        Debug.Log("ConfidenceCompleted is now true");

    }

    void LoadLobbyScene()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(lobbySceneName);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == lobbySceneName)
        {
            DestroyObjectsInLobby();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    void DestroyObjectsInLobby()
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
