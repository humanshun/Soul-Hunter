using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeSceneObject : MonoBehaviour
{
    [SerializeField] private FadeOutSceneChange fadeOutSceneChange; // フェードアウトシーンチェンジャー
    [SerializeField] private string sceneName; // 遷移先のシーン名
    [SerializeField] private GameObject wText; // プレイヤーが近くにいると表示されるテキスト

    [SerializeField] private int stageIndex; // ステージのインデックス
    [SerializeField] private Color clearedColor = Color.green; // ステージクリア時の色
    [SerializeField] private Color notClearedColor = Color.gray; // ステージ未クリア時の色
    [SerializeField] private Image targetObject; // 色を変更する対象オブジェクト
    [SerializeField] private GameObject clearObject; // クリア時に非表示にするオブジェクト
    private bool playerInTrigger = false; // プレイヤーがトリガー内にいるかどうか

    void Start()
    {
        wText.SetActive(false); // ゲーム開始時にテキストを非表示にする
        UpdateStageColor(); // ステージのクリア状況に応じて色を更新する
    }

    void Update()
    {
        // プレイヤーがトリガー内にいる場合、"w"キーが押されたときにシーンを変更する
        if (playerInTrigger && Input.GetKeyDown("w"))
        {
            if (fadeOutSceneChange != null)
            {
                fadeOutSceneChange.FadeOutAndChangeScene(sceneName); // フェードアウトしてシーンを変更する
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // プレイヤーがトリガー内に入ったときにテキストを表示する
        if (other.gameObject.CompareTag("Player"))
        {
            playerInTrigger = true;
            wText.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // プレイヤーがトリガーから出たときにテキストを非表示にする
        if (other.gameObject.CompareTag("Player"))
        {
            playerInTrigger = false;
            wText.SetActive(false);
        }
    }

    private void UpdateStageColor()
    {
        // ステージのクリア状況を確認
        bool isCleared = PlayerPrefs.GetInt("Stage_" + stageIndex, 0) == 1;

        // Rendererコンポーネントを取得して色を変更
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = isCleared ? clearedColor : notClearedColor; // ステージのクリア状況に応じた色設定
            targetObject.color = isCleared ? Color.white : Color.black; // クリアされた場合は白、そうでなければ黒
        }
        
        // クリアされた場合、指定したオブジェクトを非表示にする
        if (clearObject != null)
        {
            clearObject.SetActive(!isCleared);
        }
    }
}
