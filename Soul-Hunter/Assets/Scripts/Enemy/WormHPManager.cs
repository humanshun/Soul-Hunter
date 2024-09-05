using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormHPManager : BaseEnemyHP
{
    [SerializeField] public GameObject destoryWormPrefab; // ワームが死亡した際に生成するプレハブ

    protected override void Start()
    {
        base.Start(); // 基底クラスのStartメソッドを呼び出す
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage); // 基底クラスのTakeDamageメソッドを呼び出してダメージ処理を行う
    }

    protected override void Die()
    {
        // ワームが死亡した際にプレハブを生成する
        if (destoryWormPrefab != null) // プレハブが設定されている場合のみ生成する
        {
            Instantiate(destoryWormPrefab, transform.position, Quaternion.identity);
        }

        base.Die(); // 基底クラスのDieメソッドを呼び出す
    }
}
