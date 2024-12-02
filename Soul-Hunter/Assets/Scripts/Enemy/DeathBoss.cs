using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBoss : MonoBehaviour
{
    // ボスが死んだあと、タグをそれぞれの能力タグに変更し、プレイヤーが触れた時に能力を取得する設計
    
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
