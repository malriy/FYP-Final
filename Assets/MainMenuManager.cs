using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private DontDestroyOnLoadManager dontDestroyOnLoadManager;

    private void Start()
    {
        dontDestroyOnLoadManager = gameObject.AddComponent<DontDestroyOnLoadManager>();
        dontDestroyOnLoadManager.DestroySpecificDontDestroyOnLoadObjects();
    }
}
