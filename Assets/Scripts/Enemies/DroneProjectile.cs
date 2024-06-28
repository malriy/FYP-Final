using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneProjectile : MonoBehaviour
{
    public Vector2 Velocity;
    public float Speed;
    public Vector2 direction;
    public BoxCollider2D Collider;
    public GameObject Shooter;

    public DroneProjectile(GameObject shooter, Vector2 dir) 
    {
        Shooter = shooter;
        SetDirection(dir);
    }

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction.normalized;
        Velocity = Speed * direction;
    }

    // Start is called before the first frame update
    void Start()
    {
        Velocity = Speed * direction;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = transform.position;

        pos += Velocity * Time.deltaTime;

        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("DetectionRadius"))
        {
            Destroy(gameObject);
        }
    }
}
