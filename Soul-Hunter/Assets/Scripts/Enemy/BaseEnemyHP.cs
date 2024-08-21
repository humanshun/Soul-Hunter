using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyHP : MonoBehaviour
{
    public int maxHP = 0;
    protected int currentHP;

    public SpriteRenderer damageSprite;

    public float invincibilityDuration = 5f; // 無敵時間の長さ（秒）
    protected bool isInvincible = false; // 無敵状態を管理するフラグ

    protected virtual void Start()
    {
        currentHP = maxHP;
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

        float blinkDuration = 0.1f;  // 点滅の間隔
        float elapsedTime = 0f;

        while (elapsedTime < invincibilityDuration)
        {
            // スプライトの透明度を変更
            damageSprite.color = new Color(1f, 1f, 1f, 0f); // 透明にする
            yield return new WaitForSeconds(blinkDuration);
            
            damageSprite.color = new Color(1f, 1f, 1f, 1f); // 元に戻す
            yield return new WaitForSeconds(blinkDuration);
            
            elapsedTime += blinkDuration * 2;
        }

        // 無敵状態が終了したらスプライトの透明度を元に戻す
        damageSprite.color = new Color(1f, 1f, 1f, 1f);
        isInvincible = false;
    }
}
