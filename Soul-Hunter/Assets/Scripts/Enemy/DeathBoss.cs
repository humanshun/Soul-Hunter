using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBoss : MonoBehaviour
{
    // タグを変更するまでの秒数
    [SerializeField] private float delayInSeconds = 0.3f;

    // 変更後のタグ名
    [SerializeField] private string newTag;

    private void Start()
    {
        // 指定した秒数後にタグを変更する
        Invoke("ChangeTag", delayInSeconds);
    }

    // タグを変更するメソッド
    private void ChangeTag()
    {
        // タグを変更
        gameObject.tag = newTag;
    }
}
