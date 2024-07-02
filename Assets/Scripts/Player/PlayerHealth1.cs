using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth1 : MonoBehaviour
{
    [SerializeField] public int maxHealth = 3;
    [SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;
    [SerializeField] private Player player;

    private Slider healthSlider;
    public int currentHealth;
    private bool canTakeDamage = true;
    private KnockBack knockback;
    private Flash flash;

    private QuestManager questManager;
    [SerializeField] private Animator animator;

    protected void Awake (){
        flash = GetComponent<Flash>();
        knockback = GetComponent<KnockBack>();
        canvasGroup = deathScreen.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = deathScreen.AddComponent<CanvasGroup>();
        }
    }

    private void Start(){
        currentHealth = maxHealth;
        questManager = FindObjectOfType<QuestManager>();
        questManager.LoadQuestState();
        UpdateHealthSlider();
        deathScreen.SetActive(false);
    }

    private void OnCollisionStay2D(Collision2D other){
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();

        if(enemy && canTakeDamage){
            TakeDamage(1,other.transform);
        }
    }

    public void HealPlayer(){
        if(currentHealth < maxHealth){
            currentHealth += 3;
            UpdateHealthSlider();
        }
    }

    public void TakeDamage(int damageAmount, Transform hitTransform){
        if(!canTakeDamage){return;}

        knockback.GetKnockedBack(hitTransform,knockBackThrustAmount);
        StartCoroutine(flash.FlashRoutine());
        canTakeDamage = false;
        currentHealth -= damageAmount;
        StartCoroutine(damageRecoveryRoutine());
        UpdateHealthSlider();
        CheckIfPlayerDeath();
    }

    private void CheckIfPlayerDeath(){
        if (currentHealth <= 0){
            currentHealth = 0;
            animator.SetTrigger("playerDeath");
            player.enabled = false;
            BossAI.startedFight = false;
            StartCoroutine(OpenDeathScreen());
        }
    }

    private IEnumerator damageRecoveryRoutine(){
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private void UpdateHealthSlider(){
        if(healthSlider == null){
            healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void ResetLevel()
    {
        questManager.SaveQuestState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private GameObject deathScreen;

    private IEnumerator OpenDeathScreen()
    {
        deathScreen.SetActive(true);
        float elapsedTime = 0f;

        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1;

    }

    public void QuitButton()
    {
        questManager.SaveQuestState();
        GameObject[] persistentObjects = GameObject.FindGameObjectsWithTag("Persistent");
        foreach (GameObject obj in persistentObjects)
        {
            Destroy(obj);
        }
        SceneManager.LoadScene("MainMenu");
    }
}
