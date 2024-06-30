using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EnochStart : MonoBehaviour
{
    private void Start()
    {
        Player.Instance.transform.position = this.transform.position;
        CameraController.Instance.SetPlayerAkmalCameraFollow();
        UIFade.Instance.FadeToClear();
        
    }
}
