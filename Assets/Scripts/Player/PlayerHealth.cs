using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : Singleton<PlayerHealth>
{
    public bool isDead { get; private set; }
    [SerializeField] public int maxHealth = 3;
    [SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;
    [SerializeField] private Player player;
    [SerializeField] private Vector2 initialPosition;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button quitButton;

    private Slider healthSlider;
    public int currentHealth;
    private bool canTakeDamage = true;
    private KnockBack knockback;
    private Flash flash;

    const string HEALTH_SLIDER_TEXT = "HealthSlider";
    const string TOWN_TEXT = "Scene1";
    const string MAIN_MENU_TEXT = "MainMenu";
    readonly int DEATH_HASH = Animator.StringToHash("Death");

    [SerializeField] private Animator animator;

    protected override void Awake()
    {
        base.Awake();

        flash = GetComponent<Flash>();
        knockback = GetComponent<KnockBack>();
    }

    private void Start()
    {
        isDead = false;
        currentHealth = maxHealth;

        UpdateHealthSlider();
        if (SceneManager.GetActiveScene().name == TOWN_TEXT)
        {
            initialPosition = transform.position; 
        }

        deathScreen.SetActive(false);

        retryButton.onClick.AddListener(Retry);
        quitButton.onClick.AddListener(Quit);
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();

        if (enemy && canTakeDamage)
        {
            TakeDamage(1, other.transform);
        }
    }

    public void HealPlayer()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += 3;
            UpdateHealthSlider();
        }
    }

    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (!canTakeDamage) { return; }

        knockback.GetKnockedBack(hitTransform, knockBackThrustAmount);
        StartCoroutine(flash.FlashRoutine());
        canTakeDamage = false;
        currentHealth -= damageAmount;
        StartCoroutine(damageRecoveryRoutine());
        UpdateHealthSlider();
        CheckIfPlayerDeath();
    }

    private void CheckIfPlayerDeath()
    {
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            currentHealth = 0;
            GetComponent<Animator>().SetTrigger(DEATH_HASH);
            StartCoroutine(DeathRoutine());

            if (player.gameObject.activeInHierarchy)
            {
                player.moveSpeed = 0;
                BossAI.startedFight = false;
            }
            if (PlayerController1.Instance.gameObject.activeInHierarchy)
            {
                PlayerController1.Instance.moveSpeed = 0;
                BossAI.startedFight = false;
            }
        }
    }

    private IEnumerator DeathRoutine()
    {
        Time.timeScale = 0f; // Pause the game
        PlayerController1.Instance.enabled = false; // Disable player input
        deathScreen.SetActive(true); // Show the death screen
        yield return null;
    }

    private IEnumerator damageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private void UpdateHealthSlider()
    {
        if (healthSlider == null)
        {
            healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == TOWN_TEXT)
        {
            currentHealth = maxHealth; 
            UpdateHealthSlider();
            transform.position = initialPosition; 
            isDead = false; 

            animator.ResetTrigger(DEATH_HASH);
            animator.Play("Idle"); 

            knockback.ResetKnockBack();
        }
    }

    public void Retry()
    {
        deathScreen.SetActive(false); 
        Time.timeScale = 1f; 
        SceneManager.LoadScene(TOWN_TEXT);
    }

    public void Quit()
    {
        deathScreen.SetActive(false); 
        Time.timeScale = 1f; 
        SceneManager.LoadScene(MAIN_MENU_TEXT);
    }
}
