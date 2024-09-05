using UnityEngine;
using System.Collections;

public class GrasshopperMovement : BaseEnemyMovement
{
    private Animator anim; // アニメーターコンポーネント
    public float jumpForce = 5.0f; // ジャンプ力
    public float minJumpInterval = 3.0f; // ジャンプ間隔の最小値
    public float maxJumpInterval = 6.0f; // ジャンプ間隔の最大値
    private bool isJumping = false; // ジャンプ中かどうかのフラグ
    private bool isCoroutineRunning = false; // コルーチンが実行中かどうかのフラグ

    protected override void Start()
    {
        anim = GetComponent<Animator>(); // アニメーターコンポーネントを取得
        base.Start(); // 基底クラスのStartメソッドを呼び出す
    }

    protected override void Move()
    {
        // ジャンプ中のみ移動
        if (isJumping)
        {
            // 向きを変更するかチェック
            if (movingLeft && isFacingRight)
            {
                isFacingRight = !isFacingRight; // 右を向いている場合は左に反転
                transform.localScale = new Vector3(0.5f, 0.5f, 1); // 左を向く
            }
            else if (!movingLeft && !isFacingRight)
            {
                isFacingRight = !isFacingRight; // 左を向いている場合は右に反転
                transform.localScale = new Vector3(-0.5f, 0.5f, 1); // 右を向く
            }
            base.Move(); // 基底クラスの移動処理を呼び出す
        }
    }

    protected override void CheckDirection()
    {
        base.CheckDirection(); // 基底クラスのCheckDirectionメソッドを呼び出す
    }

    private IEnumerator JumpRoutine()
    {
        isCoroutineRunning = true; // コルーチンの開始フラグを設定
        float randomInterval = Random.Range(minJumpInterval, maxJumpInterval); // ランダムなジャンプ間隔を生成
        yield return new WaitForSeconds(randomInterval); // ジャンプ間隔待機
        anim.SetBool("IsIdle", false); // アイドルアニメーション終了
        anim.SetBool("IsPreparation", true); // ジャンプ準備アニメーション開始
        yield return new WaitForSeconds(0.5f); // ジャンプ準備時間
        anim.SetBool("IsPreparation", false); // ジャンプ準備アニメーション終了
        anim.SetBool("IsJump", true); // ジャンプアニメーション開始
        Jump(); // ジャンプを実行
        yield return new WaitForSeconds(1.0f); // ジャンプ後の待機時間
        isCoroutineRunning = false; // コルーチンの終了フラグを設定
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 地面またはトゲに接触し、コルーチンが実行されていない場合
        if ((collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Thorn")) && !isCoroutineRunning)
        {
            isJumping = false; // ジャンプ終了
            anim.SetBool("IsJump", false); // ジャンプアニメーション終了
            anim.SetBool("IsIdle", true); // アイドルアニメーション開始
            StartCoroutine(JumpRoutine()); // ジャンプのコルーチンを開始
        }
    }

    private void Jump()
    {
        if (rb != null)
        {
            isJumping = true; // ジャンプ開始フラグを設定
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); // ジャンプを実行
        }
    }
}
