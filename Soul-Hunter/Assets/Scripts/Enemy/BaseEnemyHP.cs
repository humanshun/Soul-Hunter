using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyHP : MonoBehaviour
{
    public int maxHP = 0;                      // 最大HP
    protected int currentHP;                   // 現在のHP
    public List<SpriteRenderer> damageSprites; // 被ダメージ時に使用するスプライトのリスト
    public float invincibilityDuration = 5f;   // 無敵時間の長さ
    protected bool isInvincible = false;       // 無敵状態フラグ

    protected virtual void Start()
    {
        currentHP = maxHP; // 現在のHPを最大HPに設定
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        // プレイヤーの攻撃に当たったときの処理
        if (other.gameObject.CompareTag("PlayerFoot") || other.gameObject.CompareTag("Shell") || other.gameObject.CompareTag("Bullet"))
        {
            AudioM.Instance.PlayAttackSound(); // 攻撃音を再生
            TakeDamage(1); // ダメージを受ける
        }
    }

    public virtual void TakeDamage(int damage)
    {
        // 無敵状態でない場合にダメージを受ける
        if (!isInvincible)
        {
            currentHP -= damage; // ダメージを計算

            // HPが0以下の場合は死亡処理を呼び出す
            if (currentHP <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(BecomeInvincible()); // 一定時間無敵になるコルーチンを開始
            }
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject); // 敵オブジェクトを削除
    }

    protected IEnumerator BecomeInvincible()
    {
        isInvincible = true; // 無敵状態を有効にする

        float blinkDuration = 0.1f;  // 点滅の間隔（秒）
        float elapsedTime = 0f; // 経過時間を初期化

        while (elapsedTime < invincibilityDuration)
        {
            foreach (var sprite in damageSprites)
            {
                // 各スプライトの透明度を0（透明）にする
                sprite.color = new Color(1f, 1f, 1f, 0f);
            }
            yield return new WaitForSeconds(blinkDuration); // 点滅の間隔待機

            foreach (var sprite in damageSprites)
            {
                // 各スプライトの透明度を1（不透明）に戻す
                sprite.color = new Color(1f, 1f, 1f, 1f);
            }
            yield return new WaitForSeconds(blinkDuration); // 点滅の間隔待機

            elapsedTime += blinkDuration * 2; // 経過時間を更新
        }

        // 無敵状態が終了したらすべてのスプライトの透明度を元に戻す
        foreach (var sprite in damageSprites)
        {
            sprite.color = new Color(1f, 1f, 1f, 1f);
        }
        isInvincible = false; // 無敵状態を無効にする
    }
}
