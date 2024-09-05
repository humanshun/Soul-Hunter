using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MantisBossHP : BaseEnemyHP
{
    [SerializeField] private Slider hpSlider; // HPを表示するスライダー
    [SerializeField] private ClearJudgeManager clearJudgeManager; // ボスの倒された状態を管理するクラス
    [SerializeField] private GameObject DeathMantisPrefab; // ボスの死亡時に生成するプレハブ
    private MantisMovement mantisMovement; // ボスの移動を管理するクラス
    private bool isDead = false; // 敵が既に死亡しているかどうかのフラグ

    protected override void Start()
    {
        base.Start();
        mantisMovement = GetComponent<MantisMovement>(); // MantisMovementコンポーネントを取得

        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP; // スライダーの最大値を設定
            hpSlider.value = currentHP; // スライダーの現在値を設定
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerFoot"))
        {
            TakeDamage(1); // プレイヤーの足と接触した場合、ダメージを受ける
        }
        base.OnTriggerEnter2D(other); // 親クラスのOnTriggerEnter2Dメソッドを呼び出す
    }

    public override void TakeDamage(int damage)
    {
        if (isDead) return; // 既に死亡している場合、ダメージ処理をスキップ

        // ボスが攻撃中であればダメージを無効にする
        if (mantisMovement != null && mantisMovement.IsAttacking)
        {
            return;
        }

        base.TakeDamage(damage); // 親クラスのダメージ処理を呼び出す

        if (hpSlider != null)
        {
            hpSlider.value = currentHP; // スライダーのHPを更新
        }
    }

    protected override void Die()
    {
        if (isDead) return; // 既に死亡している場合、メソッドを終了

        isDead = true; // 死亡フラグを立てる

        if (clearJudgeManager != null)
        {
            clearJudgeManager.OnBossDefeated(); // ボスが倒された際の処理を呼び出す
        }

        if (DeathMantisPrefab != null)
        {
            Instantiate(DeathMantisPrefab, transform.position, Quaternion.Euler(0, 0, 0)); // 死亡時にプレハブを生成
        }

        base.Die(); // 親クラスの死亡処理を呼び出す
    }
}
