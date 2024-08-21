using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.UI; // スライダーを使用するために必要

public class BossHPManager : BaseEnemyHP
{
    [SerializeField] private Slider hpSlider;  // スライダーの参照を追加
    [SerializeField] private ClearJugeManager clearJugeManager;
    [SerializeField] private GameObject DeathGrasshopperPrefab;
    protected override void Start()
    {
        base.Start();

        // スライダーの初期設定
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
        }
    }

    // void Update()
    // {
    //     Debug.Log(currentHP);
    // }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        // HPの変更をスライダーに反映
        if (hpSlider != null)
        {
            hpSlider.value = currentHP;
        }
    }

    protected override void Die()
    {
        if (clearJugeManager != null)
        {
            clearJugeManager.OnBossDefeated();
        }

        Instantiate(DeathGrasshopperPrefab, transform.position, Quaternion.identity);

        base.Die();
    }
}
