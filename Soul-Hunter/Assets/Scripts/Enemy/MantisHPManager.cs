using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MantisHPManager : BaseEnemyHP
{
    protected override void Start()
    {
        base.Start(); // 基底クラスのStartメソッドを呼び出す
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage); // 基底クラスのダメージ処理を呼び出す
    }

    protected override void Die()
    {
        base.Die(); // 基底クラスの死亡処理を呼び出す
    }
}
