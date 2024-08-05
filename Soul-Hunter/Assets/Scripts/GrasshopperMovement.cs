using UnityEngine;
using System.Collections;

public class GrasshopperMovement : WormMovement
{
    public float jumpForce = 5.0f; // ジャンプ力
    public float jumpInterval = 2.0f; // ジャンプ間隔
    public float jumpNextInterval = 0.5f; // ジャンプ間隔

    protected override void Start()
    {
        // 基底クラスのStartメソッドを呼び出す
        base.Start();
        // ジャンプのコルーチンを開始
        StartCoroutine(JumpRoutine());
    }

    protected override void Move()
    {
        // 基底クラスのMoveメソッドを呼び出す
        base.Move();
    }

    protected override void CheckDirection()
    {
        // 基底クラスのCheckDirectionメソッドを呼び出す
        base.CheckDirection();
    }

    private IEnumerator JumpRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(jumpInterval); // ジャンプ間隔待つ
            Jump(); // ジャンプ
            yield return new WaitForSeconds(jumpNextInterval); // ジャンプ間隔待つ
            Jump(); // ジャンプ
        }
    }

    private void Jump()
    {
        if (rb != null)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // ジャンプ
        }
    }
}
