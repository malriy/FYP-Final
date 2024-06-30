using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CityLevel : MonoBehaviour, Interactable
{
    private float waitToLoadTime = 1f;

    public void Interact()
    {
        StartCoroutine(LoadSceneRoutine());

    }

    // Start is called before the first frame update
    private IEnumerator LoadSceneRoutine()
    {
        while (waitToLoadTime >= 0)
        {
            waitToLoadTime -= Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene("Enochlophobia");
    }
}
