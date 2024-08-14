using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.UI; // スライダーを使用するために必要

public class MantisBossHP : BaseEnemyHP
{
    [SerializeField] private Slider hpSlider;  // スライダーの参照を追加
    [SerializeField] private ClearJugeManager clearJugeManager;
    [SerializeField] private GameObject DeathMantisPrefab;
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

    public override void OnTriggerEnter2D(Collider2D other)
    {
        //何かしら修正が必要。攻撃したときもダメージを受けてしまう。
        if (other.gameObject.CompareTag("PlayerFoot"))
        {
            TakeDamage(1);
        }
        base.OnTriggerEnter2D(other);
    }

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
        base.Die();
    }
    private void OnDestroy()
    {
        if (clearJugeManager != null)
        {
            clearJugeManager.OnBossDefeated();

            Instantiate(DeathMantisPrefab, transform.position, Quaternion.identity);
        }
    }
}
