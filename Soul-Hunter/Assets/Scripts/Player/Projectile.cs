using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;  // 弾の寿命

    private void Start()
    {
        // 弾の寿命が過ぎたら自動で削除する
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 弾が敵に当たった場合の処理
        if (collision.CompareTag("Enemy"))
        {
            // 敵にダメージを与えるなどの処理を実装
            Destroy(collision.gameObject); // 例: 敵を削除
            Destroy(gameObject); // 弾を削除
        }
        else if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject); // 地面に当たったら弾を削除
        }
    }
}
