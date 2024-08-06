using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyHP : MonoBehaviour
{
    public int maxHP = 0;
    protected int currentHP;
    protected Rigidbody2D rb;
    protected Collider2D col;

    protected virtual void Start()
    {
        currentHP = maxHP;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    public virtual void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
        // col.isTrigger = true;
        // rb.gravityScale = 1;
        // rb.velocity = new Vector2(0, -5);
    }
}
