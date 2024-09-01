using UnityEngine;
using System.Collections;

public class SlashAbility : Ability
{
    [SerializeField] private float slashRange = 1.0f; // 斬る攻撃の範囲
    [SerializeField] private float slashCooldown = 1.0f; // 攻撃のクールダウン時間
    [SerializeField] private int damage = 2; // 攻撃のダメージ量
    private bool canSlash = true; // 攻撃可能かどうか

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            return;
        }

        Gizmos.color = Color.red;

        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        Vector2 startPosition = (Vector2)transform.position + direction * slashRange;

        // 攻撃範囲を線で表示
        Gizmos.DrawLine(transform.position, startPosition);
    }

    public override void Activate(PlayerMovement player)
    {
        if (canSlash)
        {
            Slash(player);
        }
    }

    public override void Deactivate(PlayerMovement player)
    {
        // 斬る攻撃の無効化処理が必要な場合はこちらに記述
    }

    private void Slash(PlayerMovement player)
    {
        Animator animator = player.GetComponent<Animator>();

        // 攻撃アニメーションの再生
        animator.SetBool("MantisIsAttack", true);

        // 攻撃方向を決定
        Vector2 direction = player.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        // 攻撃範囲の計算
        Vector2 startPosition = (Vector2)player.transform.position + direction * slashRange;

        // 攻撃範囲内の敵を検出してダメージを与える
        Collider2D[] hits = Physics2D.OverlapCircleAll(startPosition, slashRange);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                // SnailHPManagerを持っているかどうかをチェック
                SnailHPManager snail = hit.GetComponentInParent<SnailHPManager>();
                if (snail != null)
                {
                    snail.TakeDamage(damage);
                    continue;
                }
                // GrasshopperHPManagerを持っているかどうかをチェック
                GrasshopperHPManager grasshopper = hit.GetComponentInParent<GrasshopperHPManager>();
                if (grasshopper != null)
                {
                    grasshopper.TakeDamage(damage);
                    continue;
                }

                // MantisHPManagerを持っているかどうかをチェック
                MantisHPManager mantis = hit.GetComponentInParent<MantisHPManager>();
                if (mantis != null)
                {
                    mantis.TakeDamage(damage);
                    continue;
                }
                // BossHPManagerを持っているかどうかをチェック
                BossHPManager Boss = hit.GetComponentInParent<BossHPManager>();
                if (Boss != null)
                {
                    Boss.TakeDamage(damage);
                    continue;
                }
                // BossHPManagerを持っているかどうかをチェック
                MantisBossHP MBoss = hit.GetComponentInParent<MantisBossHP>();
                if (MBoss != null)
                {
                    MBoss.TakeDamage(damage);
                    continue;
                }
                // 認識されないHPマネージャーの警告
                Debug.LogWarning("Enemy does not have a recognized HP Manager: " + hit.name);
            }
        }



        // アニメーション終了後にアニメーションを停止
        player.StartCoroutine(StopAnimation(animator));
        
        // 攻撃のクールダウンを開始
        canSlash = false;
        player.StartCoroutine(SlashCooldown());
    }

    private IEnumerator StopAnimation(Animator animator)
    {
        yield return new WaitForSeconds(0.15f);

        // アニメーションを元に戻す
        animator.SetBool("MantisIsAttack", false);
    }

    private IEnumerator SlashCooldown()
    {
        yield return new WaitForSeconds(slashCooldown);
        canSlash = true;
    }
}
