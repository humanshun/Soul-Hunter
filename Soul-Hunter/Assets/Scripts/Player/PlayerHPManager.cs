using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PlayerHPManager : MonoBehaviour
{
    [SerializeField] private int maxHP = 0;
    private int currentHP;
    [SerializeField] private float invincibilityDuration = 5f; // 無敵時間の長さ（秒）
    private bool isInvincible = false; // 無敵状態を管理するフラグ
    [SerializeField] private TextMeshProUGUI lifeText; // HP表示用のTextMesh Proコンポーネント
    [SerializeField] private Image[] damageImages; // HPに応じて非表示にする画像の配列
    void Start()
    {
        currentHP = maxHP;
        UpdateHPText(); // スタート時にHPテキストを更新
        InitializeDamageImages(); // スタート時に画像を表示
    }
    // void Update()
    // {
    //     Debug.Log("現在のHP : " + currentHP);
    // }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Thorn"))
        {
            TakeDamage(1);
        }
        
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            Destroy(collision.gameObject);
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
                UpdateDamageImages(); // ダメージを受けた後に画像を更新
                StartCoroutine(BecomeInvincible());
            }
        }
    }
    void Die()
    {
        GM.Instance.Life--;
        ResetDamageImages(); // 死亡時に画像をリセット
    }

    IEnumerator BecomeInvincible()
    {
        isInvincible = true;

        // プレイヤーのスプライトレンダラーを取得
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        float blinkDuration = 0.1f;  // 点滅の間隔
        float elapsedTime = 0f;

        while (elapsedTime < invincibilityDuration)
        {
            // スプライトの透明度を変更
            spriteRenderer.color = new Color(1f, 1f, 1f, 0f); // 透明にする
            yield return new WaitForSeconds(blinkDuration);
            
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f); // 元に戻す
            yield return new WaitForSeconds(blinkDuration);
            
            elapsedTime += blinkDuration * 2;
        }

        // 無敵状態が終了したらスプライトの透明度を元に戻す
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        isInvincible = false;
    }
    void UpdateHPText()
    {
        if (lifeText != null)
        {
            // HPを2桁の形式で表示（例: "03"）
            lifeText.text = "x" + GM.Instance.Life.ToString("D2");
        }
    }
    void InitializeDamageImages()
    {
        // 全ての画像を表示
        foreach (var img in damageImages)
        {
            img.enabled = true;
        }
    }
    void UpdateDamageImages()
    {
        // 現在のHPに基づいて画像を非表示にする
        for (int i = 0; i < damageImages.Length; i++)
        {
            if (i >= currentHP)
            {
                damageImages[i].enabled = false;
            }
        }
    }
    void ResetDamageImages()
    {
        // 死亡後に画像を再表示する
        InitializeDamageImages();
    }
}
