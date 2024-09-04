using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PheropsophusMovement : BaseEnemyMovement
{
    public Animator animator;
    private bool isAttacking = false;
    public Transform playerTransform;
    public Transform attackPoint;
    public GameObject BulletPrefab;
    public float bulletSpeed = 20f;
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
        yield return new WaitForSeconds(1.5f);

        AudioM.Instance.PlayShootAttackSound();

        GameObject bullet = Instantiate(BulletPrefab, attackPoint.position, Quaternion.identity);

        // プレイヤーの方向を計算する
        Vector2 direction = (playerTransform.position - attackPoint.position).normalized;

        // 弾のRigidbodyに発射方向と速度を適用
        Rigidbody2D rbody = bullet.GetComponent<Rigidbody2D>();
        rbody.velocity = direction * bulletSpeed;

        yield return new WaitForSeconds(0.3f);

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
                movingLeft = true;
            }
            else
            {
                // プレイヤーが右側にいる場合
                transform.localScale = new Vector3(-1, 1, 1); // 右を向く
                movingLeft = false;
            }
        }
    }
}
