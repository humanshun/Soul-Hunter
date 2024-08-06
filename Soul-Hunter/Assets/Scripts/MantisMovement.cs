using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MantisMovement : BaseEnemyMovement
{
    public Animator animator;
    public Collider2D targetArea;
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
    
    void OnTriggerStay2D(Collider2D other)
    {
        if (!isAttacking && targetArea != null && targetArea.IsTouching(other) && other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true; // コルーチンの開始時にフラグを設定

        animator.SetBool("IsWark", false);
        animator.SetBool("IsPreliminaryAttack", true);

        yield return new WaitForSeconds(1.0f);

        animator.SetBool("IsPreliminaryAttack", false);
        animator.SetBool("IsAttack", true);

        yield return new WaitForSeconds(1.3f);
        
        animator.SetBool("IsAttack", false);
        animator.SetBool("IsWark", true);

        isAttacking = false; // コルーチンの終了時にフラグを解除
    }
}
