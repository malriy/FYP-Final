using UnityEngine;

public class PushableBoulder : MonoBehaviour
{
    public float pushForce = 0.1f; // Adjust this to control push strength
    private Rigidbody2D rb;
    private bool isPushing = false;
    private Vector2 pushDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Prevent rotation
        rb.drag = 2; // Adjust this value to control the damping effect
    }

    void Update()
    {
        if (isPushing)
        {
            rb.velocity = pushDirection * pushForce;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            pushDirection = (collision.transform.position - transform.position).normalized;
            isPushing = true;
        }
        else if (collision.gameObject.CompareTag("Boulder"))
        {
            rb.velocity = Vector2.zero; // Stop the boulder if hit by another boulder
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            pushDirection = (collision.transform.position - transform.position).normalized;
            isPushing = true;
        }
        else if (collision.gameObject.CompareTag("Boulder"))
        {
            rb.velocity = Vector2.zero; // Stop the boulder if hit by another boulder
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPushing = false;
            rb.velocity = Vector2.zero; // Stop the boulder when not pushing
        }
    }
}
