using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour
{

    [SerializeField] private static int life = 3;
    [SerializeField] private string gameOverScene = "GameOver";
    [SerializeField] private string currentStage;

    // シングルトンインスタンス
    public static GM Instance { get; private set; }

    private void Awake()
    {
        // インスタンスがすでに存在する場合は、破棄する
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // インスタンスが存在しない場合は、このインスタンスを設定する
        Instance = this;
        
        // シーン間でオブジェクトを破棄しない
        DontDestroyOnLoad(gameObject);
    }
    // void Update()
    // {
    //     Debug.Log("現在のLife : " + life);
    // }

    public int Life
    {
        get => life;
        set
        {
            life = value;
            if (life <= 0)
            {
                life = 3;
                SceneManager.LoadScene(gameOverScene);
            }
            else
            {
                SceneManager.LoadScene(currentStage);
            }
        }
    }
}
