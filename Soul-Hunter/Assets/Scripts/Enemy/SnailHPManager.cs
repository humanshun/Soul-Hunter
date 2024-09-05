using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailHPManager : BaseEnemyHP
{
    public GameObject ShellPrefab; // 殻のプレハブ

    protected override void Start()
    {
        base.Start(); // 基底クラスのStartメソッドを呼び出す
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage); // 基底クラスのTakeDamageメソッドを呼び出す
    }

    protected override void Die()
    {
        base.Die(); // 基底クラスのDieメソッドを呼び出す

        // 殻のプレハブを生成する
        if (ShellPrefab != null) // プレハブが設定されている場合のみ生成する
        {
            Instantiate(ShellPrefab, transform.position, transform.rotation);
        }
    }
}
