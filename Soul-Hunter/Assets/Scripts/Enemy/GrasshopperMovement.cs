using UnityEngine;
using System.Collections;

public class GrasshopperMovement : BaseEnemyMovement
{
    private Animator anim;
    public float jumpForce = 5.0f; // ジャンプ力
    public float minJumpInterval = 3.0f; // ジャンプ間隔の最小値
    public float maxJumpInterval = 6.0f; // ジャンプ間隔の最大値
    private bool isJumping = false; // ジャンプ中かどうかのフラグ
    private bool isCoroutineRunning = false; // コルーチンが実行中かどうかのフラグ

    protected override void Start()
    {
        anim = GetComponent<Animator>();
        // 基底クラスのStartメソッドを呼び出す
        base.Start();
    }

    protected override void Move()
    {
        // ジャンプ中のみ移動
        if (isJumping)
        {
            // 向きを変更するかチェック
            if (movingLeft && isFacingRight)
            {
                isFacingRight = !isFacingRight;
                transform.localScale = new Vector3(0.5f, 0.5f, 1); // 反転して左を向く
            }
            else if (!movingLeft && !isFacingRight)
            {
                isFacingRight = !isFacingRight;
                transform.localScale = new Vector3(-0.5f, 0.5f, 1); // 右を向く
            }
            base.Move();
        }
    }

    protected override void CheckDirection()
    {
        // 基底クラスのCheckDirectionメソッドを呼び出す
        base.CheckDirection();
    }

    private IEnumerator JumpRoutine()
    {
        isCoroutineRunning = true; // コルーチン開始
        float randomInterval = Random.Range(minJumpInterval, maxJumpInterval); // ランダムなジャンプ間隔を生成
        yield return new WaitForSeconds(randomInterval); // ジャンプ間隔待つ
        anim.SetBool("IsIdle", false);
        anim.SetBool("IsPreparation", true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("IsPreparation", false);
        anim.SetBool("IsJump", true);
        Jump(); // ジャンプ
        yield return new WaitForSeconds(1.0f);
        Jump(); // ジャンプ
        isCoroutineRunning = false; // コルーチン終了
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Thorn")) && !isCoroutineRunning)
        {
            isJumping = false; // 地面に接触したらジャンプ終了
            anim.SetBool("IsJump", false);
            anim.SetBool("IsIdle", true);
            StartCoroutine(JumpRoutine()); // ジャンプのコルーチンを開始
        }
    }

    private void Jump()
    {
        if (rb != null)
        {
            isJumping = true; // ジャンプ開始
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // ジャンプ
        }
    }
}
