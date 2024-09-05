using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class GrasshopperHPManager : BaseEnemyHP
{
    [SerializeField] private GameObject DeathGrasshopperPrefab; // 死亡時に生成するプレハブ

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage); // 親クラスのダメージ処理を呼び出す
    }

    protected override void Die()
    {
        // 敵が死亡した際にプレハブを生成
        Instantiate(DeathGrasshopperPrefab, transform.position, Quaternion.identity); 

        base.Die(); // 親クラスの死亡処理を呼び出す
    }
}
