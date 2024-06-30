using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
	public float detectionRange = 10f;
	public float attackRange = 2f;
	public float speed = 2f;
	public float attackCooldown;
	private float playerDistance;
	public int damage = 10;
	public static bool startedFight = false;

	private Transform player;
	private Rigidbody2D rb;
	private Vector2 movement;
	private bool isAttacking = false;
	private bool isMoving = false;
	public float lastAttackTime = 0f;

	[SerializeField] private Animator animator, attackAnimator;
	[SerializeField] private GameObject healthSlider;

    public GameObject rocketPrefab;
	public Transform rocketPos;
	public GameObject fireHitbox;

	private enum State
	{
		Idle,
		Chase,
		Attack
	}

	private State currentState = State.Idle;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();

		StartCoroutine(StateMachine());
	}

	private void Update()
	{
		playerDistance = Vector2.Distance(player.position, transform.position);
		animator.SetFloat("speed", speed);
		animator.SetBool("startedFight", startedFight);

        if (!isMoving)
        {
            speed = 0;
        }
        else
		{
			speed = 2f;
		}
	}

	private IEnumerator StateMachine()
	{
		while (true)
		{
			switch (currentState)
			{
				case State.Idle:
					Idle();
					break;
				case State.Chase:
					Chase();
					break;
				case State.Attack:
					Attack();
                    break;
			}
			yield return new WaitForFixedUpdate();
		}
	}

	private void Idle()
	{
		if (startedFight)
		{
			healthSlider.SetActive(true);
			currentState = State.Chase;
		}
	}

	private void Chase()
	{
        CheckAndFlip();
		isMoving = true;

        if (playerDistance >= attackRange)
		{
            // Continue chasing the player
            Vector2 direction = (player.position - transform.position).normalized;
            rb.MovePosition((Vector2)transform.position + (direction * speed * Time.fixedDeltaTime));
        }   

		if (Time.time >= lastAttackTime + attackCooldown)
		{
			currentState = State.Attack;
		}
	}


	private void Attack()
	{
        isAttacking = true;
        isMoving = false;

        // Determine attack based on player distance
        if (playerDistance >= attackRange)
        {
            StartCoroutine(Attack1());
        }
        else
        {
            StartCoroutine(Attack2());
        }

        CheckAndFlip();
	}

	// Ranged Attack
	private IEnumerator Attack1()
	{
		attackAnimator.SetTrigger("attack1");
		yield return new WaitForSeconds(attackCooldown);
		isAttacking = false;
        currentState = State.Chase;
    }

    private void FireRocket()
    {
        Vector3 hole = rocketPos.position;
        Vector2 playerPos = player.position;
        Vector2 direction = (playerPos - (Vector2)rocketPos.position).normalized;

        GameObject shot = Instantiate(rocketPrefab, hole, Quaternion.identity);
        BossProjectile rocket = shot.GetComponent<BossProjectile>();
        rocket.SetDirection(direction);
    }

    private bool playerInHitbox = false;
    private IEnumerator Attack2()
	{
		attackAnimator.SetTrigger("attack2");

		if (playerInHitbox)
		{
            PlayerHealth health = player.GetComponent<PlayerHealth>();
			health.TakeDamage(5, transform);
        }

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
        currentState = State.Chase;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInHitbox = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInHitbox = false;
        }
    }

    private void Flip()
	{
		// Flip the boss and all its children horizontally
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;

		foreach (Transform child in transform)
		{
			Vector3 childScale = child.localScale;
			childScale.x *= -1;
			child.localScale = childScale;

			SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
			if (sr != null)
			{
                if (sr.flipX)
                {
                    sr.flipX = false;
                }
                else
                {
                    sr.flipX = true;
                }
            }
		}
	}

	private void CheckAndFlip()
	{
		// Determine the direction the boss is facing
		float bossDirection = transform.localScale.x;

		// Check if the player is behind the boss
		if ((bossDirection > 0 && player.position.x < transform.position.x) || (bossDirection < 0 && player.position.x > transform.position.x))
		{
			Flip();
		}
	}
}
