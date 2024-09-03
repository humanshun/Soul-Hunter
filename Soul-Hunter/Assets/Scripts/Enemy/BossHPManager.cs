using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.UI;

public class BossHPManager : BaseEnemyHP
{
    [SerializeField] private Slider hpSlider;
    [SerializeField] private ClearJudgeManager clearJudgeManager; // スペル修正
    [SerializeField] private GameObject deathPrefab; // プレハブ名を小文字に修正
    [SerializeField] private float quaternionZOffset = 0f;

    protected override void Start()
    {
        base.Start();

        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (hpSlider != null)
        {
            hpSlider.value = currentHP;
        }
    }

    protected override void Die()
    {
        if (clearJudgeManager != null)
        {
            clearJudgeManager.OnBossDefeated();
        }

        if (deathPrefab != null) // null チェック追加
        {
            Instantiate(deathPrefab, transform.position, Quaternion.Euler(0, 0, quaternionZOffset));
        }

        base.Die();
    }
}
