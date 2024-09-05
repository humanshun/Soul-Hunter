using UnityEngine;
using System.Collections;

public class ShootAbility : Ability
{
    [SerializeField] private GameObject projectilePrefab;  // 弾のプレハブ
    [SerializeField] private Transform firePoint;          // 弾を発射する位置
    [SerializeField] private float baseProjectileSpeed = 10f;   // 基本弾の速度
    [SerializeField] private float maxProjectileSpeed = 30f;    // 最大弾の速度
    [SerializeField] private float shootCooldown = 1.0f;    // クールダウン時間
    [SerializeField] private float upwardAngle = 15f; // 上方向の角度

    private bool canShoot = true;  // 発射可能かどうか

    public override void Activate(PlayerMovement player)
    {
        // 弾を発射
        if (canShoot)
        {
            Shoot(player, 0f); // デフォルトのチャージ時間で発射
        }
    }

    public override void Deactivate(PlayerMovement player)
    {
        Shoot[] projectiles = FindObjectsOfType<Shoot>(); // すべての弾を取得

        foreach (Shoot projectile in projectiles)
        {
            Destroy(projectile.gameObject); // 弾を削除
        }

        Debug.Log("Projectile ability deactivated."); // アビリティが無効化されたことをログに出力
    }

    public void Shoot(PlayerMovement player, float chargeTime)
    {
        if (!canShoot)
        {
            return; // クールダウン中は発射をスキップ
        }

        Animator animator = player.GetComponent<Animator>();

        // 攻撃アニメーションの再生
        animator.SetBool("PheropsophusIsAttack", true);

        // 0.5秒後に PheropsophusIsAttack を false にする
        player.StartCoroutine(ResetAttackAnimation(animator));

        // チャージされた速度を計算
        float projectileSpeed = Mathf.Lerp(baseProjectileSpeed, maxProjectileSpeed, chargeTime);

        // プレイヤーの向きを確認し、発射方向を設定
        Vector2 fireDirection = player.transform.localScale.x > 0 ? firePoint.right : -firePoint.right;

        // 発射方向に上向きの角度を追加
        Quaternion rotation = Quaternion.Euler(0, 0, player.transform.localScale.x > 0 ? upwardAngle : -upwardAngle);
        fireDirection = rotation * fireDirection;

        // 弾を生成
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // 音を再生
        AudioM.Instance.PlayShootAttackSound();

        // 弾に力を加えて発射
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = fireDirection * projectileSpeed;
        }

        // クールダウンを開始
        canShoot = false;
        player.StartCoroutine(ShootCooldown());
    }

    private IEnumerator ResetAttackAnimation(Animator animator)
    {
        yield return new WaitForSeconds(0.5f); // 0.5秒待つ
        animator.SetBool("PheropsophusIsAttack", false); // PheropsophusIsAttack を false にする
    }

    private IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(shootCooldown); // クールダウン時間を待つ
        canShoot = true; // 発射可能にする
    }
}
