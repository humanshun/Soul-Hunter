using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyHP : MonoBehaviour
{
    public int maxHP = 0;
    protected int currentHP;
    protected Rigidbody2D rb;
    protected Collider2D col;

    public float invincibilityDuration = 5f; // 無敵時間の長さ（秒）
    protected bool isInvincible = false; // 無敵状態を管理するフラグ

    protected virtual void Start()
    {
        currentHP = maxHP;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerFoot"))
        {
            TakeDamage(1);
        }
    }

    public virtual void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            currentHP -= damage;

            if (currentHP <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(BecomeInvincible());
            }
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected IEnumerator BecomeInvincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }
}
