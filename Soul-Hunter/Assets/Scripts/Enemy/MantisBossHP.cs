using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MantisBossHP : BaseEnemyHP
{
    [SerializeField] private Slider hpSlider;
    [SerializeField] private ClearJudgeManager clearJudgeManager;
    [SerializeField] private GameObject DeathMantisPrefab;
    private MantisMovement mantisMovement;
    private bool isDead = false; // 敵が既に死亡しているかどうかのフラグ

    protected override void Start()
    {
        base.Start();
        mantisMovement = GetComponent<MantisMovement>();

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
        if (isDead) return; // 既に死亡している場合、ダメージ処理をスキップ

        if (mantisMovement != null && mantisMovement.IsAttacking)
        {
            return;
        }

        base.TakeDamage(damage);

        if (hpSlider != null)
        {
            hpSlider.value = currentHP;
        }
    }

    protected override void Die()
    {
        if (isDead) return; // 既に死亡している場合、メソッドを終了

        isDead = true; // 死亡フラグを立てる

        if (clearJudgeManager != null)
        {
            clearJudgeManager.OnBossDefeated();
        }

        if (DeathMantisPrefab != null)
        {
            Instantiate(DeathMantisPrefab, transform.position, Quaternion.Euler(0, 0, 0));
        }

        base.Die();
    }
}
