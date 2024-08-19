using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PheropsophusMovement : BaseEnemyMovement
{
    public Animator animator;
    private bool isAttacking = false; // フラグを追加
    protected override void Move()
    {
        if (isAttacking) // 攻撃中は移動しない
        {
            rb.velocity = new Vector2(0, rb.velocity.y); // 速度を0に設定
            return;
        }

        base.Move(); // 通常の移動処理を行う
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Attack());
        }
    }
    private IEnumerator Attack()
    {
        isAttacking = true; // コルーチンの開始時にフラグを設定

        animator.SetBool("Attack", true);

        // コルーチンが終了する前に、一定の時間（アニメーションが終わるのにかかる時間）待つ
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        animator.SetBool("Attack", false);

        isAttacking = false; // コルーチンの終了時にフラグを解除
    }
}
