using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI; // ボタンにアクセスするために必要

public class GM : MonoBehaviour
{
    [SerializeField] private static int life = 3;
    [SerializeField] private string gameOverScene = "GameOver";
    [SerializeField] private string currentStage;
    [SerializeField] private GameObject pauseMenu; // ポーズメニューのUI
    [SerializeField] private Button resumeButton;  // Buttonコンポーネントに変更
    [SerializeField] private Button restartButton; // Buttonコンポーネントに変更
    [SerializeField] private Button optionButton;
    [SerializeField] private Button stageSelectButton;
    [SerializeField] private Button exitButton;    // Buttonコンポーネントに変更
    [SerializeField] private GameObject[] imageObjects;
    [SerializeField] private int currentStageIndex;

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
        resumeButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        optionButton.gameObject.SetActive(false);
        stageSelectButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);

        foreach (GameObject image in imageObjects)
        {
            image.SetActive(false);
        }

        // ボタンにリスナーを登録
        resumeButton.onClick.AddListener(OnResumeButtonClicked);
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        optionButton.onClick.AddListener(OnOptionButtonClicked);
        stageSelectButton.onClick.AddListener(OnStageSelectButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
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
            resumeButton.gameObject.SetActive(!isPaused);
            restartButton.gameObject.SetActive(!isPaused);
            optionButton.gameObject.SetActive(!isPaused);
            stageSelectButton.gameObject.SetActive(!isPaused);
            exitButton.gameObject.SetActive(!isPaused);

            foreach (GameObject image in imageObjects)
            {
                image.SetActive(!isPaused);
            }
            
            Time.timeScale = isPaused ? 1f : 0f; // ゲームを一時停止または再開

            if (!isPaused && EventSystem.current != null)
            {
                // ポーズメニューが表示されたときに最初に選択するボタンを設定
                EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
            }
        }
    }

    // 「再開」ボタンが押されたときの処理
    private void OnResumeButtonClicked()
    {
        TogglePause();
    }

    // 「再スタート」ボタンが押されたときの処理
    private void OnRestartButtonClicked()
    {
        TogglePause();
        SceneManager.LoadScene(currentStage); // 現在のステージを再ロード
    }

    private void OnOptionButtonClicked()
    {
        
    }

    private void OnStageSelectButtonClicked()
    {
        TogglePause();
        SceneManager.LoadScene("StageSelect");
    }

    // 「終了」ボタンが押されたときの処理
    private void OnExitButtonClicked()
    {
        TogglePause();
        SceneManager.LoadScene("Title"); // メインメニューに戻る
    }

    // ステージクリア時に呼び出すメソッド
    public void OnStageCleared()
    {
        // 現在のステージ番号を使ってクリア状況を保存
        PlayerPrefs.SetInt("Stage_" + currentStageIndex, 1); // クリア済みとして保存
        PlayerPrefs.Save();
    }
}
