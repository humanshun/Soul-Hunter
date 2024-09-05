using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.UI;

public class BossHPManager : BaseEnemyHP
{
    [SerializeField] private Slider hpSlider; // ボスのHPを表示するスライダー
    [SerializeField] private ClearJudgeManager clearJudgeManager; // ボスの撃破を判断するマネージャー
    [SerializeField] private GameObject deathPrefab; // 死亡時に生成するプレハブ
    [SerializeField] private float quaternionZOffset = 0f; // 回転角度のオフセット

    protected override void Start()
    {
        base.Start(); // 親クラスのStartメソッドを呼び出す

        // HPスライダーの設定
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP; // スライダーの最大値を設定
            hpSlider.value = currentHP; // スライダーの現在値を設定
        }
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage); // 親クラスのダメージ処理を呼び出す

        // HPスライダーの更新
        if (hpSlider != null)
        {
            hpSlider.value = currentHP; // スライダーの値を現在のHPに更新
        }
    }

    protected override void Die()
    {
        // ボスが倒されたときの処理
        if (clearJudgeManager != null)
        {
            clearJudgeManager.OnBossDefeated(); // ボスの撃破を通知
        }

        // 死亡時のエフェクトを生成
        if (deathPrefab != null) // null チェック
        {
            Instantiate(deathPrefab, transform.position, Quaternion.Euler(0, 0, quaternionZOffset)); // プレハブを生成
        }

        base.Die(); // 親クラスの死亡処理を呼び出す
    }
}
