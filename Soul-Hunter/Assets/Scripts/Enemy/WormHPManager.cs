using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormHPManager : BaseEnemyHP
{
    protected override void Start()
    {
        base.Start();
    }
    // void Update()
    // {
    //     Debug.Log(currentHP);
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
