using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CityLevel : MonoBehaviour, Interactable
{
    public void Interact()
    {
        SceneManager.LoadScene("Enochlophobia");
    }
}
