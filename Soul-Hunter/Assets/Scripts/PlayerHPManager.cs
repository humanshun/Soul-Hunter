using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHPManager : MonoBehaviour
{
    public int maxHP = 0;
    private int currentHP;
    public float invincibilityDuration = 5f; // 無敵時間の長さ（秒）
    private bool isInvincible = false; // 無敵状態を管理するフラグ
    void Start()
    {
        currentHP = maxHP;
    }
    // void Update()
    // {
    //     Debug.Log("現在のHP : " + currentHP);
    // }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }
    public void TakeDamage(int damage)
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
    void Die()
    {
        GM.Instance.Life--;
    }

    IEnumerator BecomeInvincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }
}
