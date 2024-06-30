using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] public int EnemyID;
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private GameObject deathVFXPrefab;
    [SerializeField] private float knockBackThrust = 15f;
    [SerializeField] private Animator animator;
    [SerializeField] private string deathAnim;
    [SerializeField] private Player player;

    public delegate void EnemyDiedEventHandler(EnemyHealth enemy);
    public static event EnemyDiedEventHandler OnEnemyDied;

    private int currentHealth;
    private KnockBack knockBack;
    private Flash flash;

    public int CurrentHealth
    {
        get { return currentHealth; }
    }

    private void Awake()
    {
        flash = GetComponent<Flash>();
        knockBack = GetComponent<KnockBack>();
    }

    private void Start()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        knockBack.GetKnockedBack((PlayerController1.Instance != null ? PlayerController1.Instance.transform : player.transform), knockBackThrust);
        StartCoroutine(flash.FlashRoutine());
        StartCoroutine(CheckDetectDeathRoutine());
    }

    private IEnumerator CheckDetectDeathRoutine()
    {
        yield return new WaitForSeconds(flash.GetRestoreMatTime());
        DetectDeath();
    }

    public void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            if (animator != null)
            {
                animator.SetTrigger(deathAnim);
            }
            else if (deathVFXPrefab != null) 
            {
                Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
                GetComponent<PickUpSpawner>().DropItems();
            }
            OnEnemyDied?.Invoke(this);
            Destroy(gameObject);
        }
    }
}

