using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailHPManager : BaseEnemyHP
{
    protected override void Start()
    {
        base.Start();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    protected override void Die()
    {
        base.Die();
    }
}
