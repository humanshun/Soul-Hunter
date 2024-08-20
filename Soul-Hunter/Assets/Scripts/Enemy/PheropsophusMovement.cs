using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PheropsophusMovement : BaseEnemyMovement
{
    public Animator animator;
    private bool isAttacking = false;
    public Transform playerTransform;
    public float detectionRange = 5f; // プレイヤーを検出する距離(威嚇の距離)
    public float attackRange = 2f; // プレイヤーを検出する距離(攻撃の距離)

    protected override void Move()
    {
        if (isAttacking) // 攻撃中は移動しない
        {
            rb.velocity = new Vector2(0, rb.velocity.y); // 速度を0に設定
            return;
        }

        base.Move(); // 通常の移動処理を行う

        DistancePlayer();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerFoot"))
        {
            StartCoroutine(Attack());
        }
    }

    

    private void DistancePlayer() //playerとの距離を測って距離によってアニメーションを変える
    {
        if (playerTransform != null && !isAttacking) //playerがいて、アタック中じゃないとき
        {
            float distance = Vector2.Distance(transform.position, playerTransform.position); //距離を測って

            if (distance <= detectionRange) //もし威嚇距離以内にプレイヤーがいたら
            {
                rb.velocity = new Vector2(0, rb.velocity.y); //速度を0に設定
                animator.SetBool("Preliminary", true); //威嚇モーションをtrue
                FacePlayer(); //プレイヤーの方向を向く

                if (distance <= attackRange) //もし攻撃距離以内にいたら
                {
                    StartCoroutine(Attack());
                }
            }
            else
            {
                animator.SetBool("Preliminary", false);
            }
        }
    }


    private IEnumerator Attack() //アタックメソッド
    {
        isAttacking = true;

        animator.SetBool("Attack", true);

        // アニメーションが終了するまで待つ
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        animator.SetBool("Attack", false);

        isAttacking = false;
    }

    private void FacePlayer() //プレイヤーの方向を向くメソッド
    {
        if (playerTransform != null)
        {
            // 敵とプレイヤーのx座標を比較して、敵がプレイヤーの方向を向くようにする
            if (playerTransform.position.x < transform.position.x)
            {
                // プレイヤーが左側にいる場合
                transform.localScale = new Vector3(1, 1, 1); // 左を向く
            }
            else
            {
                // プレイヤーが右側にいる場合
                transform.localScale = new Vector3(-1, 1, 1); // 右を向く
            }
        }
    }
}
