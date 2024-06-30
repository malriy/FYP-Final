using UnityEngine;

public class EnochStart : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject park;  // Assign this in the Unity editor
    private static EnochStart instance;

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

        player.transform.position = this.transform.position;
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
