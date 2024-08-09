using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormDeathMovement : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DestroyTime());
    }
    IEnumerator DestroyTime()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}
