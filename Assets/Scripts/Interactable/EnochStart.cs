using UnityEngine;

public class EnochStart : MonoBehaviour
{
    [SerializeField] private GameObject park;  // Assign this in the Unity editor
    private static EnochStart instance;
    public static bool EnochCompleted;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (park != null)
        {
            park.SetActive(false);
        }
        CameraController.Instance.SetPlayerAkmalCameraFollow();
        UIFade.Instance.FadeToClear();
    }

    public static void ActivatePark()
    {
        if (instance != null && instance.park != null)
        {
            instance.park.SetActive(true);
        }
    }
}
