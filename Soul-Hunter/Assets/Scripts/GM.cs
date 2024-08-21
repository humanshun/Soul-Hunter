using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GM : MonoBehaviour
{
    [SerializeField] private static int life = 3;
    [SerializeField] private string gameOverScene = "GameOver";
    [SerializeField] private string currentStage;
    [SerializeField] private GameObject pauseMenu; // ポーズメニューのUI
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject titleButton;

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

    private void Start()
    {
        pauseMenu.SetActive(false);
        backButton.SetActive(false);
        restartButton.SetActive(false);
        titleButton.SetActive(false);
    }

    private void Update()
    {
        // ESCキーが押されたらポーズ画面をトグル
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

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

    // ポーズ画面のトグル処理
    private void TogglePause()
    {
        if (pauseMenu != null)
        {
            bool isPaused = pauseMenu.activeSelf;
            pauseMenu.SetActive(!isPaused);
            backButton.SetActive(!isPaused);
            restartButton.SetActive(!isPaused);
            titleButton.SetActive(!isPaused);
            
            Time.timeScale = isPaused ? 1f : 0f; // ゲームを一時停止または再開

            if (!isPaused && EventSystem.current != null)
            {
                // ポーズメニューが表示されたときに最初に選択するボタンを設定
                EventSystem.current.SetSelectedGameObject(backButton);
            }
        }
    }
}
