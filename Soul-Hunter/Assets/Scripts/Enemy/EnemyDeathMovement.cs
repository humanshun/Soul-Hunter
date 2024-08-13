using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathMovement : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DestroyTime());
    }
    IEnumerator DestroyTime()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }
}
