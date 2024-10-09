using UnityEngine;
using System.Collections;

public class GrasshopperBossMovement : BaseEnemyMovement
{
    private Animator anim; // Animatorコンポーネントの参照
    public float jumpForce = 5.0f; // ジャンプ力
    public float minJumpInterval = 3.0f; // ジャンプ間隔の最小値
    public float maxJumpInterval = 6.0f; // ジャンプ間隔の最大値
    private bool isJumping = false; // ジャンプ中かどうかのフラグ

    protected override void Start()
    {
        anim = GetComponent<Animator>(); // Animatorコンポーネントの取得
        base.Start(); // 基底クラスのStartメソッドを呼び出す
    }

    protected override void Move()
    {
        // ジャンプ中のみ移動する
        if (isJumping)
        {
            // 向きを変更するかチェック
            if (movingLeft && isFacingRight)
            {
                isFacingRight = !isFacingRight;
                transform.localScale = new Vector3(1, 1, 1); // 反転して左を向く
            }
            else if (!movingLeft && !isFacingRight)
            {
                isFacingRight = !isFacingRight;
                transform.localScale = new Vector3(-1, 1, 1); // 右を向く
            }
            base.Move(); // 基底クラスのMoveメソッドを呼び出す
        }
    }

    protected override void CheckDirection()
    {
        // 基底クラスのCheckDirectionメソッドを呼び出す
        base.CheckDirection();
    }

    private IEnumerator JumpRoutine()
    {
        float randomInterval = Random.Range(minJumpInterval, maxJumpInterval); // ランダムなジャンプ間隔を生成
        yield return new WaitForSeconds(randomInterval); // ジャンプ間隔待つ
        anim.SetBool("IsIdle", false); // アイドル状態を解除
        anim.SetBool("IsPreparation", true); // 準備状態にする
        yield return new WaitForSeconds(0.5f); // 準備の待機時間
        anim.SetBool("IsPreparation", false); // 準備状態を解除
        anim.SetBool("IsJump", true); // ジャンプ状態にする
        Jump(); // ジャンプを実行
        yield return new WaitForSeconds(1.0f); // ジャンプの待機時間
        Jump(); // もう一度ジャンプを実行
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false; // 地面に接触したらジャンプ終了
            anim.SetBool("IsJump", false); // ジャンプ状態を解除
            anim.SetBool("IsIdle", true); // アイドル状態にする
            StartCoroutine(JumpRoutine()); // ジャンプのコルーチンを開始
        }
        base.OnCollisionEnter2D(collision);
    }

    private void Jump()
    {
        if (rb != null)
        {
            isJumping = true; // ジャンプ開始
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // 上方向に力を加えてジャンプ
        }
    }
}
