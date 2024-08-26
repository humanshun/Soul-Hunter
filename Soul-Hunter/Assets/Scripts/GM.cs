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
    [SerializeField] private GameObject optionMenu; // サブメニューのUIオブジェクト

    // シングルトンインスタンス
    public static GM Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
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
        optionMenu.SetActive(false); // サブメニューを初期状態で非表示にする

        foreach (GameObject image in imageObjects)
        {
            image.SetActive(false);
        }

        resumeButton.onClick.AddListener(OnResumeButtonClicked);
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        optionButton.onClick.AddListener(OnOptionButtonClicked);
        stageSelectButton.onClick.AddListener(OnStageSelectButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void Update()
    {
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
            // optionMenu.SetActive(false); // ポーズを解除したときにはサブメニューを非表示にする

            foreach (GameObject image in imageObjects)
            {
                image.SetActive(!isPaused);
            }

            Time.timeScale = isPaused ? 1f : 0f;

            if (!isPaused && EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
            }
        }
    }

    private void OnResumeButtonClicked()
    {
        TogglePause();
    }

    private void OnRestartButtonClicked()
    {
        TogglePause();
        SceneManager.LoadScene(currentStage);
    }

    // 「オプション」ボタンが押されたときの処理
    private void OnOptionButtonClicked()
    {
        TogglePause();
        if (optionMenu != null)
        {
            bool isOptionMenuActive = optionMenu.activeSelf;
            optionMenu.SetActive(!isOptionMenuActive); // サブメニューをトグル表示
            Time.timeScale = isOptionMenuActive ? 1f : 0f;
        }
    }

    private void OnStageSelectButtonClicked()
    {
        TogglePause();
        SceneManager.LoadScene("StageSelect");
    }

    private void OnExitButtonClicked()
    {
        TogglePause();
        SceneManager.LoadScene("Title");
    }

    public void OnStageCleared()
    {
        PlayerPrefs.SetInt("Stage_" + currentStageIndex, 1);
        PlayerPrefs.Save();
    }
}

