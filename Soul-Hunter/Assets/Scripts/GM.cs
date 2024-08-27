using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GM : MonoBehaviour
{
    [SerializeField] private static int life = 3;
    [SerializeField] private string gameOverScene = "GameOver";
    [SerializeField] private string currentStage;
    [SerializeField] private int currentStageIndex;
    private bool isPaused;
    private bool isOptionMenuActive;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button stageSelectButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject[] pauseImageObject;
    [SerializeField] private GameObject optionMenu;
    [SerializeField] private Button backPauseButton;
    [SerializeField] private Button soundButton;
    [SerializeField] private Button operateButton;
    [SerializeField] private GameObject[] opsionImageObjects;

    private GameObject lastSelected;

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
        isPaused = true;
        isOptionMenuActive = true;
        pauseMenu.SetActive(false);
        resumeButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        optionButton.gameObject.SetActive(false);
        stageSelectButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);

        foreach (GameObject image in pauseImageObject)
        {
            image.SetActive(false);
        }

        optionMenu.SetActive(false);
        backPauseButton.gameObject.SetActive(false);
        soundButton.gameObject.SetActive(false);
        operateButton.gameObject.SetActive(false);

        foreach (GameObject image in opsionImageObjects)
        {
            image.SetActive(false);
        }

        resumeButton.onClick.AddListener(OnResumeButtonClicked);
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        optionButton.onClick.AddListener(OnOptionButtonClicked);
        stageSelectButton.onClick.AddListener(OnStageSelectButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);

        backPauseButton.onClick.AddListener(OnBackPauseButtonClicked);
        soundButton.onClick.AddListener(OnSoundButtonClicked);
        operateButton.onClick.AddListener(OnOperateButtonClicked);
    }

    private void Update()
    {
        HandlePauseToggle();

        HandleButtonSelectionChange();
    }

    private void HandlePauseToggle()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                // ゲームがポーズされていない場合の処理
            }
            else if (!isOptionMenuActive)
            {
                // オプションメニューがアクティブではない場合の処理
            }
            else
            {
                TogglePause();
            }
        }
        Debug.Log("ポーズ" + isPaused);
        Debug.Log("オプション" + isOptionMenuActive);
    }

    private void HandleButtonSelectionChange()
    {
        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

        if (currentSelected != null && currentSelected != lastSelected)
        {
            if (currentSelected.GetComponent<Button>() != null)
            {
                AudioM.Instance.PlayButtonSelectSound();
                lastSelected = currentSelected;
            }
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
            isPaused = pauseMenu.activeSelf;
            pauseMenu.SetActive(!isPaused);
            resumeButton.gameObject.SetActive(!isPaused);
            restartButton.gameObject.SetActive(!isPaused);
            optionButton.gameObject.SetActive(!isPaused);
            stageSelectButton.gameObject.SetActive(!isPaused);
            exitButton.gameObject.SetActive(!isPaused);
            optionMenu.SetActive(false);

            foreach (GameObject image in pauseImageObject)
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

    private void OnOptionButtonClicked()
    {
        TogglePause();
        Opsion();
    }

    private void Opsion()
    {
        if (optionMenu != null)
        {
            bool isOptionMenuActive = optionMenu.activeSelf;
            optionMenu.SetActive(!isOptionMenuActive);
            backPauseButton.gameObject.SetActive(!isOptionMenuActive);
            soundButton.gameObject.SetActive(!isOptionMenuActive);
            operateButton.gameObject.SetActive(!isOptionMenuActive);

            foreach (GameObject image in opsionImageObjects)
            {
                image.SetActive(!isOptionMenuActive);
            }

            Time.timeScale = isOptionMenuActive ? 0f : 0f;

            if (!isOptionMenuActive && EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(backPauseButton.gameObject);
            }
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

    private void OnBackPauseButtonClicked()
    {
        Opsion();
        TogglePause();
    }

    private void OnSoundButtonClicked()
    {
        Opsion();
    }

    private void OnOperateButtonClicked()
    {

    }

    public void OnStageCleared()
    {
        PlayerPrefs.SetInt("Stage_" + currentStageIndex, 1);
        PlayerPrefs.Save();
    }
}
