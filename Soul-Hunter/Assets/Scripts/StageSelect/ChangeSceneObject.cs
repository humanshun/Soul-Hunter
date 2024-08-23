using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneObject : MonoBehaviour
{
    [SerializeField] private FadeOutSceneChange fadeOutSceneChange;
    [SerializeField] private string sceneName; // SceneName -> sceneNameに変更
    [SerializeField] private GameObject wText;
    private bool playerInTrigger = false;

    void Start()
    {
        wText.SetActive(false);
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
}
