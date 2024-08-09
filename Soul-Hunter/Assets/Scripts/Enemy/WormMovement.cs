using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarmMovement : BaseEnemyMovement
{
    [SerializeField] public GameObject destoryWormPrefab;
    void OnDestroy()
    {
        if (destoryWormPrefab != null)
        {
        // 0.1秒遅延させてリスポーンさせる
        Invoke(nameof(RespawnPrefab), 0.1f);
        }
    }
    void RespawnPrefab()
    {
        Instantiate(destoryWormPrefab, transform.position, Quaternion.identity);
    }
}
