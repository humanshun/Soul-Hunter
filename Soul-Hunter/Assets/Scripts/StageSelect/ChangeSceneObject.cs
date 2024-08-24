using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneObject : MonoBehaviour
{
    [SerializeField] private FadeOutSceneChange fadeOutSceneChange;
    [SerializeField] private string sceneName; // SceneName -> sceneNameに変更
    [SerializeField] private GameObject wText;

    [SerializeField] private int stageIndex;
    [SerializeField] private Color clearedColor = Color.green;
    [SerializeField] private Color notClearedColor = Color.gray;
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
        }
    }
}
