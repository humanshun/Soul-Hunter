using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormHPManager : BaseEnemyHP
{
    protected override void Start()
    {
        maxHP = 0;
        base.Start();
    }
    // void Update()
    // {
    //     Debug.Log(maxHP);
    // }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    protected override void Die()
    {
        base.Die();
    }
}
