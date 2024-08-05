using UnityEngine;

public class WormMovement : MonoBehaviour
{
    public float speed = 2.0f; //移動速度
    public Transform pointA;   //折り返し地点A
    public Transform pointB;   //折り返し地点B

    protected Rigidbody2D rb;
    protected Vector2 direction; //移動方向
    private bool movingLeft = true; // 現在の移動方向を示すフラグ
    private bool isFacingRight = false; // 現在の向きを示すフラグ

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Move();
        CheckDirection();
    }

    protected virtual void Move()
    {
        // 移動方向に応じて速度を設定
        if (movingLeft)
        {
            // 左に移動
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
        else
        {
            // 右に移動
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }

        // 向きを変更するかチェック
        if (movingLeft && isFacingRight)
        {
            Flip();
        }
        else if (!movingLeft && !isFacingRight)
        {
            Flip();
        }
    }

    protected virtual void CheckDirection()
    {
        // 左に移動中で左の境界を超えた場合
        if (movingLeft && transform.position.x <= pointA.position.x)
        {
            // 移動方向を右に変更
            movingLeft = false;
        }
        // 右に移動中で右の境界を超えた場合
        else if (!movingLeft && transform.position.x >= pointB.position.x)
        {
            // 移動方向を左に変更
            movingLeft = true;
        }
    }

    protected void Flip() //向きを反転させるメソッド
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
}
