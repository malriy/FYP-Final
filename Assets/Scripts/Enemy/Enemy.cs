using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int ID;

    public Animator animator;
    public Transform player;
    public Transform shotLocation;

    //Shooting
    public GameObject projectilePrefab;
    public float fireCooldown = 0f;
    protected float fireRate = 0f;
    public Vector2 direction;
    public CircleCollider2D circleCollider;
    private Coroutine attackCoroutine;
    public bool attacking = false;

    //Moving and Roaming
    protected float elapsedTime;

    //Enemy Stats
    public float MS;
    public float newMS;
    private Vector3 initialPosition;
    private bool roaming = true;
    public float Health
    {
        set
        {
            health = value;
            if (health <= 0)
            {
                EnemyDie();
            }
        }
        get { return health; }
    }

    public float health;

    private enum RoamingState
    {
        Idle, // Drone is not moving
        Moving // Drone is currently moving
    }
    private RoamingState roamingState = RoamingState.Idle;

    private void Start()
    {
        GameObject playerObject = GameObject.Find("Player");

        // Check if the player GameObject was found
        if (playerObject != null)
        {
            // Get the transform component of the player GameObject
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player GameObject not found!");
        }
    }

    private void FixedUpdate()
    {
        if (player != null && !attacking)
        {
            Roam();
            Debug.Log(roamingState);
        }

        if (attacking)
        {
            CheckAndFlip();
        }
    }

    private Coroutine fireCoroutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Player entered the detection radius, start attacking
            attacking = true;
            animator.SetBool("droneCanShoot", true);
            roaming = false;
            MS = 0f;

            // Start firing coroutine
            if (fireCoroutine == null)
            {
                fireCoroutine = StartCoroutine(FireMissiles());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            attacking = false;
            animator.SetBool("droneCanShoot", false);

            // Stop firing coroutine
            if (fireCoroutine != null)
            {
                StopCoroutine(fireCoroutine);
                fireCoroutine = null;
            }

            Roam();
            roaming = true;
            MS = newMS;
        }
    }

    public static event Action<Enemy> OnEnemyDied;
    public void EnemyDie()
    {
        animator.SetTrigger("droneDeath");
        OnEnemyDied?.Invoke(this);
        DestroyEnemy();
    }

    void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    void FireMissile()
    {
        Vector3 spawnPosition = shotLocation.position;
        Vector2 playerPos = player.position;
        Vector2 direction = (playerPos - (Vector2)shotLocation.position).normalized;

        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        DroneProjectile shot = projectile.GetComponent<DroneProjectile>();
        shot.SetDirection(direction);

        Destroy(projectile, 3f);
    }

    IEnumerator FireMissiles()
    {
        while (attacking)
        {
            //FireMissile();
            yield return new WaitForSeconds(fireRate); // Adjust fireRate as needed
        }
    }


    Vector3 targetPosition;
    public float roamRange = 10f;
    private float movementThreshold = 0.1f;
    public LayerMask wallLayer;
    public float wallCheckDistance = 1f;

    private void Roam()
    {
        roaming = true;
        if (roamingState == RoamingState.Idle)
        {
            // Calculate a new random target position within the roam range
            float roamRange = 10f;
            targetPosition = transform.position + new Vector3(UnityEngine.Random.Range(-roamRange, roamRange), 0f, 0f);

            // Update the roaming state to indicate that the drone is moving
            roamingState = RoamingState.Moving;
        }
        else if (roamingState == RoamingState.Moving)
        {
            // Move towards the target position
            float movement = MS * Time.deltaTime;
            float moveDirection = targetPosition.x > transform.position.x ? 1f : -1f; // Determine the direction based on the target position

            // Translate the enemy
            transform.Translate(Vector2.right * moveDirection * movement);

            // Flip the enemy if needed
            if ((moveDirection > 0 && transform.localScale.x < 0) || (moveDirection < 0 && transform.localScale.x > 0))
            {
                Flip();
            }

            // Check if the drone has reached its target position
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
            if (distanceToTarget < movementThreshold)
            {
                roamingState = RoamingState.Idle;
            }

            // Set animation speed for roaming
            animator.SetFloat("Speed", MS);
        }
    }

    private void Flip()
    {
        // Flip the enemy horizontally
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        shotLocation.Rotate(0f, 180f, 0f);
    }

    private void CheckAndFlip()
    {
        // Determine the direction the enemy is facing
        float enemyDirection = transform.localScale.x;

        // Check if the player is behind the enemy
        if ((enemyDirection > 0 && player.position.x < transform.position.x) || (enemyDirection < 0 && player.position.x > transform.position.x))
        {
            Flip();
        }
    }

    
}
