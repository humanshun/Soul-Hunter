using UnityEngine;

public class GrasshopperMovement : WormMovement
{
    public float jumpForce = 5f; // ジャンプ力
    public float jumpInterval = 3f; // ジャンプ間隔
    private float nextJumpTime;

    protected override void Start()
    {
        base.Start();
        nextJumpTime = Time.time + jumpInterval; // 最初のジャンプ時間を設定
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        // 現在の時間が次のジャンプ時間を超えた場合
        if (Time.time >= nextJumpTime)
        {
            Jump();
            nextJumpTime = Time.time + jumpInterval; // 次のジャンプ時間を設定
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // ジャンプする
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }
}