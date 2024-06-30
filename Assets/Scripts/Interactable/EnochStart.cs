using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EnochStart : MonoBehaviour
{
    [SerializeField] private Player player;

    private void Start()
    {
        player.transform.position = this.transform.position;
        CameraController.Instance.SetPlayerAkmalCameraFollow();
        UIFade.Instance.FadeToClear();
        
    }
}
