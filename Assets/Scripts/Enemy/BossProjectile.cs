using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public Vector2 Velocity;
    public float Speed;
    public Vector2 direction;
    public BoxCollider2D Collider;
    public GameObject Shooter;
    public Animator animator;

    private Vector3 destination;
    private Player player;
    [SerializeField] private GameObject explosionPrefab;

    public BossProjectile(GameObject shooter, Vector2 dir)
    {
        Shooter = shooter;
        SetDirection(dir);
    }

    private void SetRotation(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction.normalized;
        Velocity = Speed * direction;
        SetRotation(direction);
    }

    // Start is called before the first frame update
    void Start()
    {
        Velocity = Speed * direction;
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player").GetComponent<Player>();

        SetRotation(direction);
        SetDestination();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = transform.position;

        pos += Velocity * Time.deltaTime;

        transform.position = pos;
        float distance = Vector2.Distance(transform.position, destination);
        float threshold = 0.1f; 

        if (distance <= threshold)
        {
            OnHit();
        }
    }

    private void SetDestination()
    {
        Vector3 dest = new Vector3 (player.transform.position.x, player.transform.position.y, player.transform.position.z);
        destination = dest;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnHit();
        }
    }

    private void OnHit()
    {
        Velocity = Vector2.zero;
        Destroy(gameObject);

        GameObject explosion = Instantiate(explosionPrefab, destination, Quaternion.identity);
    }
}
