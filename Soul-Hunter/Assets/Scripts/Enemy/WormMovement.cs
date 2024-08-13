using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarmMovement : BaseEnemyMovement
{
    [SerializeField] public GameObject destoryWormPrefab;
    void OnDestroy()
    {
        Instantiate(destoryWormPrefab, transform.position, Quaternion.identity);
    }
}
