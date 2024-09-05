using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    [SerializeField] private string nextStage; // 次のステージ名
    [SerializeField] private GameM gameM; // ゲームマネージャーの参照

    // 衝突が発生したときに呼ばれるメソッド
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 衝突したオブジェクトが「Player」タグを持っているか確認
        if (collision.gameObject.CompareTag("Player"))
        {
            // ステージクリアの処理を実行
            gameM.OnStageCleared();
            // 次のステージに遷移
            SceneManager.LoadScene(nextStage);
        }
    }
}
