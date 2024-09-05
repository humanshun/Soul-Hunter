using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathMovement : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DestroyTime()); // 一定時間後にオブジェクトを破壊するコルーチンを開始
    }

    IEnumerator DestroyTime()
    {
        // 2秒間待機
        yield return new WaitForSeconds(2.0f);
        
        // オブジェクトを破壊
        Destroy(gameObject);
    }
}
