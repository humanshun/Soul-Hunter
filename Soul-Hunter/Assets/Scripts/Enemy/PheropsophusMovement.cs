using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PheropsophusMovement : BaseEnemyMovement
{
    public Animator animator; // アニメーターコンポーネント
    private bool isAttacking = false; // 攻撃中かどうかのフラグ
    public Transform playerTransform; // プレイヤーの位置
    public Transform attackPoint; // 弾を発射する位置
    public GameObject BulletPrefab; // 弾のプレハブ
    public float bulletSpeed = 20f; // 弾の速度
    public float detectionRange = 5f; // プレイヤーを検出する距離 (威嚇の距離)
    public float attackRange = 2f; // プレイヤーを検出する距離 (攻撃の距離)

    protected override void Move()
    {
        if (isAttacking) // 攻撃中は移動しない
        {
            rb.velocity = new Vector2(0, rb.velocity.y); // 水平速度を0に設定
            return;
        }

        base.Move(); // 通常の移動処理を行う

        DistancePlayer(); // プレイヤーとの距離に基づいた処理
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerFoot"))
        {
            StartCoroutine(Attack()); // プレイヤーに接触したら攻撃開始
        }
    }

    private void DistancePlayer() // プレイヤーとの距離を測定し、アニメーションを変更する
    {
        if (playerTransform != null && !isAttacking) // プレイヤーが存在し、攻撃中でない場合
        {
            float distance = Vector2.Distance(transform.position, playerTransform.position); // 距離を測定

            if (distance <= detectionRange) // 威嚇距離以内にプレイヤーがいる場合
            {
                rb.velocity = new Vector2(0, rb.velocity.y); // 水平速度を0に設定
                animator.SetBool("Preliminary", true); // 威嚇モーションを設定
                FacePlayer(); // プレイヤーの方向を向く

                if (distance <= attackRange) // 攻撃距離以内にプレイヤーがいる場合
                {
                    StartCoroutine(Attack()); // 攻撃を開始
                }
            }
            else
            {
                animator.SetBool("Preliminary", false); // 威嚇モーションを解除
            }
        }
    }

    private IEnumerator Attack() // 攻撃処理
    {
        isAttacking = true; // 攻撃フラグを立てる

        animator.SetBool("Attack", true); // 攻撃アニメーションを設定

        yield return new WaitForSeconds(1.5f); // アニメーションが終了するまで待機

        AudioM.Instance.PlayShootAttackSound(); // 攻撃音を再生

        // 弾を発射する
        GameObject bullet = Instantiate(BulletPrefab, attackPoint.position, Quaternion.identity);

        // プレイヤーの方向を計算する
        Vector2 direction = (playerTransform.position - attackPoint.position).normalized;

        // 弾のRigidbodyに発射方向と速度を設定
        Rigidbody2D rbody = bullet.GetComponent<Rigidbody2D>();
        rbody.velocity = direction * bulletSpeed;

        yield return new WaitForSeconds(0.3f); // 少し待機

        animator.SetBool("Attack", false); // 攻撃アニメーションを解除

        isAttacking = false; // 攻撃フラグを解除
    }

    private void FacePlayer() // プレイヤーの方向を向く
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
