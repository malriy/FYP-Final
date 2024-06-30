using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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

    [SerializeField] private Animator animator;

    protected void Awake (){


        flash = GetComponent<Flash>();
        knockback = GetComponent<KnockBack>();
    }

    private void Start(){
        currentHealth = maxHealth;

        UpdateHealthSlider();
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

            if (player.gameObject.activeInHierarchy)
            {
                player.moveSpeed = 0;
                BossAI.startedFight = false;
            }
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
}
