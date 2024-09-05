using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player; // プレイヤーのTransformを指定します
    public Vector3 offset;   // プレイヤーからのオフセットを指定します

    void Update()
    {
        // オブジェクトの位置をプレイヤーの位置にオフセットを加えた位置に設定します
        transform.position = player.position + offset;
    }
}
