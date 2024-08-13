using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MantisMovement : BaseEnemyMovement
{
    public Animator animator;
    public float minJumpInterval = 3.0f; // 間隔の最小値
    public float maxJumpInterval = 6.0f; // 間隔の最大値
    private bool isAttacking = false; // フラグを追加

    protected override void Start()
    {
        StartCoroutine(RandomCoroutine());
        base.Start();
    }
    protected override void Move()
    {
        if (isAttacking) // 攻撃中は移動しない
        {
            rb.velocity = new Vector2(0, rb.velocity.y); // 速度を0に設定
            return;
        }

        base.Move(); // 通常の移動処理を行う
    }
    
    private IEnumerator RandomCoroutine()
    {
        float randomInterval = Random.Range(minJumpInterval, maxJumpInterval);
        yield return new WaitForSeconds(randomInterval);
        StartCoroutine(Attack());
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
        StartCoroutine(RandomCoroutine());
    }
}
