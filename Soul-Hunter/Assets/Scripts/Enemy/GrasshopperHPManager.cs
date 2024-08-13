using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class GrasshopperHPManager : BaseEnemyHP
{
    [SerializeField] private GameObject DeathGrasshopperPrefab;

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    protected override void Die()
    {
        base.Die();
    }
    private void OnDestroy()
    {
        Instantiate(DeathGrasshopperPrefab, transform.position, Quaternion.identity);
    }
}
