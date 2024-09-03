using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MantisBossHP : BaseEnemyHP
{
    [SerializeField] private Slider hpSlider;
    [SerializeField] private ClearJudgeManager clearJudgeManager;
    [SerializeField] private GameObject DeathMantisPrefab;
    private MantisMovement mantisMovement; // MantisMovementの参照を追加

    protected override void Start()
    {
        base.Start();

        mantisMovement = GetComponent<MantisMovement>(); // MantisMovementのコンポーネントを取得

        // スライダーの初期設定
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerFoot"))
        {
            TakeDamage(1);
        }
        base.OnTriggerEnter2D(other);
    }

    public override void TakeDamage(int damage)
    {
        // 攻撃中でない場合のみダメージを受ける
        if (mantisMovement != null && mantisMovement.IsAttacking)
        {
            return;
        }

        base.TakeDamage(damage);

        // HPの変更をスライダーに反映
        if (hpSlider != null)
        {
            hpSlider.value = currentHP;
        }
    }

    protected override void Die()
    {
        base.Die();

        if (clearJudgeManager != null)
        {
            clearJudgeManager.OnBossDefeated();
        }
    }
}
