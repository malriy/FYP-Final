using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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


    private bool canTakeDamage = true;
    [SerializeField] private float damageRecoveryTime = 1f;

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
        animator.SetTrigger("isDead");
    }

    private void DestroyBoss()
    {
        Destroy(gameObject);
    }

    public int GetHealth()
    {
        return currentHealth;
    }
}
