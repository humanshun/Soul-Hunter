using UnityEngine;

public class ShootAbility : Ability
{
    [SerializeField] private GameObject projectilePrefab;  // 弾のプレハブ
    [SerializeField] private Transform firePoint;           // 弾を発射する位置
    [SerializeField] private float projectileSpeed = 10f;   // 弾の速度

    public override void Activate(PlayerMovement player)
    {
        // 弾を生成する
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // 弾に力を加えて発射する
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = firePoint.right * projectileSpeed;
        }
    }

    public override void Deactivate(PlayerMovement player)
    {
        // 弾を削除するなどの処理
        // ここでは例として、アクティブな弾を全て削除する処理を示します。
        Shoot[] projectiles = FindObjectsOfType<Shoot>(); // すべての弾を取得する

        foreach (Shoot projectile in projectiles)
        {
            Destroy(projectile.gameObject); // 弾を削除する
        }

        // 他の必要な処理があればここに追加
        Debug.Log("Projectile ability deactivated.");
    }
}
