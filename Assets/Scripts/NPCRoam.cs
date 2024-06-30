using UnityEngine;
using System.Collections;

public class NPCRoam : MonoBehaviour
{
    public float speed = 2f; // Speed of the NPC
    public float moveDistance = 5f; // Distance the NPC will move before changing direction
    public float waitTime = 2f; // Time to wait before resuming movement at the end of distance
    public bool enableWaitAtEnd = false; // Option to enable waiting at the end distance

    private bool movingRight = true;
    private float startPositionX;
    private SpriteRenderer spriteRenderer;
    private bool isPaused = false; // To check if NPC is paused
    private bool isWaiting = false; // To check if NPC is waiting to resume
    private Animator animator;

    void Start()
    {
        startPositionX = transform.position.x;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isPaused && !isWaiting)
        {
            Move();
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }

    void Move()
    {
        // Move the NPC left or right
        if (movingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (transform.position.x >= startPositionX + moveDistance)
            {
                if (enableWaitAtEnd)
                {
                    StartCoroutine(WaitAtEnd());
                }
                else
                {
                    Flip();
                    movingRight = false;
                }
            }
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (transform.position.x <= startPositionX - moveDistance)
            {
                if (enableWaitAtEnd)
                {
                    StartCoroutine(WaitAtEnd());
                }
                else
                {
                    Flip();
                    movingRight = true;
                }
            }
        }
    }

    void Flip()
    {
        // Flip the NPC sprite
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && IsFacingPlayer(collision.transform))
        {
            isPaused = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPaused = false;
            if (!isWaiting)
            {
                StartCoroutine(WaitBeforeResuming());
            }
        }
    }

    private bool IsFacingPlayer(Transform player)
    {
        // Check if NPC is facing the player
        float directionToPlayer = player.position.x - transform.position.x;
        bool isFacingRight = movingRight && directionToPlayer > 0;
        bool isFacingLeft = !movingRight && directionToPlayer < 0;

        if (isFacingRight || isFacingLeft)
        {
            return true;
        }

        return false;
    }

    IEnumerator WaitBeforeResuming()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
    }

    IEnumerator WaitAtEnd()
    {
        isWaiting = true;
        animator.SetBool("moving", false);
        yield return new WaitForSeconds(waitTime);
        animator.SetBool("moving", true);
        isWaiting = false;
        Flip();
        movingRight = !movingRight;
    }
}
