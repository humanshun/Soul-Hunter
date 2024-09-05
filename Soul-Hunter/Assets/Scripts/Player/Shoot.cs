using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;  // 弾の寿命
    private Rigidbody2D rb; // Rigidbody2Dコンポーネント

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2Dコンポーネントの取得
        // 弾の寿命が過ぎたら自動で削除する
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // 移動方向がある場合のみ回転を行う
        if (rb.velocity != Vector2.zero)
        {
            // 移動方向を取得
            Vector2 direction = rb.velocity.normalized;
            
            // 移動方向に対してスプライトを回転させる
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle + 90); // 90度補正してスプライトの向きを調整
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 弾が敵に当たった場合の処理
        if (collision.CompareTag("Enemy"))
        {
            Destroy(gameObject); // 弾を削除
        }
        else if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject); // 地面に当たったら弾を削除
        }
    }
}
