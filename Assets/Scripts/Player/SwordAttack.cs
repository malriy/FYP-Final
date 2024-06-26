using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public float damage = 3;
    Vector2 rightAttackOffset;
    public Collider2D swordCollider;

    private void Start()
    {
        swordCollider.enabled = false;
        rightAttackOffset = new Vector2(0.8f, -0.7f);
    }

    public void AttackRight()
    {
        Debug.Log("hi");

        swordCollider.enabled = true;
        transform.localPosition = rightAttackOffset;
    }

    public void AttackLeft()
    {
        Debug.Log("hey");
        swordCollider.enabled = true;
        transform.localPosition = new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y);
    }

    public void StopAttack()
    {
        swordCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Health -= damage;
            }

            BossStats boss = collision.GetComponent<BossStats>();
            if (boss != null)
            {
                boss.TakeDamage((int)damage);
            }
        }
    }
}
