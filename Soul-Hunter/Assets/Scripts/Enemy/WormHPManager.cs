using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormHPManager : BaseEnemyHP
{
    [SerializeField] public GameObject destoryWormPrefab;
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
        Instantiate(destoryWormPrefab, transform.position, Quaternion.identity);
        base.Die();
    }
}
