using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossStats : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private Slider slider;

    public Animator animator;
    public GameObject head;
    [SerializeField] private Flash flash;
    [SerializeField] private Flash headFlash;
    [SerializeField] private GameObject endFlash;


    private bool canTakeDamage = true;
    [SerializeField] private float damageRecoveryTime = 1f;

    private void Awake()
    {
        canvasGroup = endFlash.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = endFlash.AddComponent<CanvasGroup>();
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        //slider.gameObject.SetActive(false);
        UpdateHealthSlider();
    }

    void Update()
    {
        // Any updates for the boss stats can go here if needed.
    }

    public void TakeDamage(int amount)
    {
        if (!canTakeDamage) { return; }
        currentHealth -= amount;

        StartCoroutine(flash.FlashRoutine());
        StartCoroutine(headFlash.FlashRoutine());

        canTakeDamage = false;
        StartCoroutine(damageRecoveryRoutine());
        UpdateHealthSlider();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthSlider()
    {
        if (slider == null)
        {
            slider = GameObject.Find("BossSlider").GetComponent<Slider>();
        }

        slider.maxValue = maxHealth;
        slider.value = currentHealth;
    }

    private IEnumerator damageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private void Die()
    {
        head.SetActive(false);
        EnochStart.EnochCompleted = true;
        BossAI ai = GetComponent<BossAI>();
        ai.enabled = false;
        animator.SetTrigger("isDead");
    }

    private IEnumerator DestroyBoss()
    {
        yield return new WaitForSeconds(0.5f);
        EndLevel();
        Destroy(gameObject);
    }

    private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    public void EndLevel()
    {
        GameObject[] persistentObjects = GameObject.FindGameObjectsWithTag("Persistent");
        foreach (GameObject obj in persistentObjects)
        {
            Destroy(obj);
        }
        SceneManager.LoadScene("Lobby");
        StartNPC.lastLevelCompleted = 2;
    }

    private IEnumerator FlashAndDestroy()
    {
        endFlash.SetActive(true);
        slider.gameObject.SetActive(false);
        //Boss flash explosion
        float elapsedTime = 0f;

        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = 1 - Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0;
        endFlash.SetActive(false);
        
    }

    public int GetHealth()
    {
        return currentHealth;
    }
}
