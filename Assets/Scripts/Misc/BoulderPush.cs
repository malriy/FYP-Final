using UnityEngine;

public class PushableBoulder : MonoBehaviour
{
    public float pushForce = 0.1f; // Adjust this to control push strength
    private Rigidbody2D rb;
    private bool isPushing = false;
    private Vector2 pushDirection;
    private bool isStoppedByBoulder = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Prevent rotation
        rb.drag = 2; // Adjust this value to control the damping effect
    }

    void Update()
    {
        if (isPushing && !isStoppedByBoulder)
        {
            rb.velocity = pushDirection * pushForce;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            pushDirection = (transform.position - collision.transform.position).normalized;
            isPushing = true;
        }
        else if (collision.gameObject.CompareTag("Boulder"))
        {
            StopBothBoulders(collision);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            pushDirection = (transform.position - collision.transform.position).normalized;
            isPushing = true;
        }
        else if (collision.gameObject.CompareTag("Boulder"))
        {
            StopBothBoulders(collision);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPushing = false;
            rb.velocity = Vector2.zero; // Stop the boulder when not pushing
        }
        else if (collision.gameObject.CompareTag("Boulder"))
        {
            isStoppedByBoulder = false;
            // Reset the other boulder's state if it was stopped by this one
            PushableBoulder otherBoulder = collision.gameObject.GetComponent<PushableBoulder>();
            if (otherBoulder != null)
            {
                otherBoulder.isStoppedByBoulder = false;
                otherBoulder.rb.velocity = Vector2.zero;
            }
        }
    }

    private void StopBothBoulders(Collision2D collision)
    {
        rb.velocity = Vector2.zero; // Stop this boulder
        isPushing = false;
        isStoppedByBoulder = true;

        // Stop the other boulder
        PushableBoulder otherBoulder = collision.gameObject.GetComponent<PushableBoulder>();
        if (otherBoulder != null)
        {
            otherBoulder.isPushing = false;
            otherBoulder.isStoppedByBoulder = true;
            otherBoulder.rb.velocity = Vector2.zero;
        }
    }
}
