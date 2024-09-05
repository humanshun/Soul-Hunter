using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletMovement : MonoBehaviour
{
    private Vector2 direction; // 弾の移動方向
    private Rigidbody2D rb; // Rigidbody2D コンポーネント

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D コンポーネントを取得
    }

    void Update()
    {
        // 弾が移動している場合のみ回転を行う
        if (rb.velocity != Vector2.zero)
        {
            // 現在の移動方向を取得
            Vector2 direction = rb.velocity.normalized;
            
            // 移動方向に基づいてスプライトを回転させる
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 弧度法を度数法に変換
            transform.rotation = Quaternion.Euler(0, 0, angle + 90); // 回転角度を適用
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 何かに衝突した場合、弾を破壊する
        Destroy(gameObject);
    }
}
