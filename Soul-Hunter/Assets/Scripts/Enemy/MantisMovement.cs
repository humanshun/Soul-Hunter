using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MantisMovement : BaseEnemyMovement
{
    public Animator animator; // アニメーターコンポーネント
    public float minAttackInterval = 3.0f; // 攻撃の間隔の最小値
    public float maxAttackInterval = 6.0f; // 攻撃の間隔の最大値
    private bool isAttacking = false; // 攻撃中かどうかのフラグ

    public bool IsAttacking => isAttacking; // isAttackingのプロパティを公開

    protected override void Start()
    {
        StartCoroutine(RandomCoroutine()); // ランダムな攻撃タイミングのコルーチンを開始
        base.Start(); // 基底クラスのStartメソッドを呼び出す
    }

    protected override void Move()
    {
        if (isAttacking)
        {
            // 攻撃中は水平移動を停止
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        base.Move(); // 基底クラスの移動処理を呼び出す
    }
    
    private IEnumerator RandomCoroutine()
    {
        // ランダムな攻撃間隔を生成
        float randomInterval = Random.Range(minAttackInterval, maxAttackInterval);
        yield return new WaitForSeconds(randomInterval); // 指定した間隔を待機
        StartCoroutine(Attack()); // 攻撃のコルーチンを開始
    }

    private IEnumerator Attack()
    {
        isAttacking = true; // 攻撃フラグを立てる

        // 攻撃アニメーションの設定
        animator.SetBool("IsWalk", false);
        animator.SetBool("IsPreliminaryAttack", true);

        yield return new WaitForSeconds(1.0f); // 攻撃準備時間

        animator.SetBool("IsPreliminaryAttack", false);
        animator.SetBool("IsAttack", true); // 実際の攻撃アニメーションを開始
        AudioM.Instance.PlayMantisAttackSound(); // 攻撃音を再生

        yield return new WaitForSeconds(1.3f); // 攻撃の持続時間

        // 攻撃終了後の設定
        animator.SetBool("IsAttack", false);
        animator.SetBool("IsWalk", true);

        isAttacking = false; // 攻撃フラグを解除
        StartCoroutine(RandomCoroutine()); // 次の攻撃のコルーチンを再開
    }
}
