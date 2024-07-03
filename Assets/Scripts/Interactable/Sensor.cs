using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sensor : MonoBehaviour
{
    [SerializeField] private GameObject endingScene;
    private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    private void Start()
    {
        canvasGroup = endingScene.GetComponent<CanvasGroup>();
    }

    public void Quit()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(showEnding());
        }
    }

    private IEnumerator showEnding()
    {
        endingScene.SetActive(true);
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
}
