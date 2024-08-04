using UnityEngine;

public class SnailMovement : MonoBehaviour
{
    public float speed = 2.0f; //移動速度
    public Transform pointA;   //折り返し地点A
    public Transform pointB;   //折り返し地点B

    private Rigidbody2D rb;
    private Vector2 direction; //移動方向

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = Vector2.left; //最初の移動方向を左に設定
    }

    void FixedUpdate()
    {
        rb.velocity = direction * speed; //rbの横方向に向きと速度を設定
    }

    void OnTriggerEnter2D(Collider2D other) //Triggerに当たった時のメソッド
    {
        if (other.transform == pointA || other.transform == pointB)
        {
            direction *= -1; //移動方向を逆にする
            Flip(); //向きを反転
        }
    }

    void Flip() //向きを反転させるメソッド
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
