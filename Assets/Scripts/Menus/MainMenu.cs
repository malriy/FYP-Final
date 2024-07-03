using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject howToPlay;
    private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    private void Start()
    {
        canvasGroup = howToPlay.GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            float elapsedTime = 0f;

            elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = 1 - Mathf.Clamp01(elapsedTime / fadeDuration);
            }
            canvasGroup.alpha = 0;
            howToPlay.SetActive(false);
        }
    }

    public void Play()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void HowToPlay()
    {
        StartCoroutine(showHTP());
    }

    private IEnumerator showHTP()
    {
        howToPlay.SetActive(true);
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
