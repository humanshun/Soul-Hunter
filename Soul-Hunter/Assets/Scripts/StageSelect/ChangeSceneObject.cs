using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeSceneObject : MonoBehaviour
{
    [SerializeField] private FadeOutSceneChange fadeOutSceneChange;
    [SerializeField] private string sceneName;
    [SerializeField] private GameObject wText;

    [SerializeField] private int stageIndex;
    [SerializeField] private Color clearedColor = Color.green;
    [SerializeField] private Color notClearedColor = Color.gray;
    [SerializeField] private Image targetObject; // 追加: 色を変更する対象オブジェクト
    [SerializeField] private GameObject clearObject; // 追加: クリア時に非表示にするオブジェクト
    private bool playerInTrigger = false;

    void Start()
    {
        wText.SetActive(false);
        UpdateStageColor();
    }

    void Update()
    {
        if (playerInTrigger && Input.GetKeyDown("w"))
        {
            if (fadeOutSceneChange != null)
            {
                fadeOutSceneChange.FadeOutAndChangeScene(sceneName);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInTrigger = true;
            wText.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
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
            renderer.material.color = isCleared ? clearedColor : notClearedColor;
            targetObject.color = isCleared ? Color.white : Color.black; // クリアされている場合は白、そうでなければ灰色
        }
        
        // クリアされた場合、指定したオブジェクトを非表示にする
        if (clearObject != null)
        {
            clearObject.SetActive(!isCleared);
        }
    }
}
