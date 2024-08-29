using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameM : MonoBehaviour
{
    [SerializeField] private static int life = 3;
    [SerializeField] private string gameOverScene = "GameOver";
    [SerializeField] private string currentStage;
    [SerializeField] private int currentStageIndex;
    private bool isPaused;
    private bool isOptionMenu;
    private bool isSoundMenu;
    private bool isOperationMenu;
    [SerializeField] private GameObject pauseMenuText;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button stageSelectButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject[] pauseImageObject;


    [SerializeField] private GameObject optionMenuText;
    [SerializeField] private Button backPauseButton;
    [SerializeField] private Button soundButton;
    [SerializeField] private Button operateButton;
    [SerializeField] private GameObject[] opsionImageObjects;


    [SerializeField] private GameObject soundMenuText;
    [SerializeField] private Button backOpsionButton_1;
    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SESlider;
    [SerializeField] private GameObject[] soundImageObjects;

    [SerializeField] private GameObject operationMenuText;
    [SerializeField] private Button backOpsionButton_2;
    [SerializeField] private Image operationImage;
    [SerializeField] private GameObject[] operateImageObject;

    private GameObject lastSelected;

    public static GameM Instance { get; private set; }

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
        // スライダーの初期値設定
        BGMSlider.value = AudioM.Instance.BGMVolume; // BGMの音量初期値
        SESlider.value = AudioM.Instance.EffectsVolume; // 効果音の音量初期値

        isPaused = true;
        isOptionMenu = true;
        isSoundMenu = true;
        isOperationMenu = true;


        //ポーズ
        pauseMenuText.SetActive(false);
        resumeButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        optionButton.gameObject.SetActive(false);
        stageSelectButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);

        foreach (GameObject image in pauseImageObject)
        {
            image.SetActive(false);
        }

        //オプション
        optionMenuText.SetActive(false);
        backPauseButton.gameObject.SetActive(false);
        soundButton.gameObject.SetActive(false);
        operateButton.gameObject.SetActive(false);

        foreach (GameObject image in opsionImageObjects)
        {
            image.SetActive(false);
        }

        //サウンド
        soundMenuText.SetActive(false);
        backOpsionButton_1.gameObject.SetActive(false);
        BGMSlider.gameObject.SetActive(false);
        SESlider.gameObject.SetActive(false);

        foreach (GameObject image in soundImageObjects)
        {
            image.SetActive(false);
        }

        //操作方法
        operationMenuText.SetActive(false);
        backOpsionButton_2.gameObject.SetActive(false);
        operationImage.gameObject.SetActive(false);

        foreach (GameObject image in operateImageObject)
        {
            image.SetActive(false);
        }

        //ポーズ
        resumeButton.onClick.AddListener(OnResumeButtonClicked);
        resumeButton.onClick.AddListener(AudioM.Instance.PlayButtonClickSound);
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        restartButton.onClick.AddListener(AudioM.Instance.PlayButtonClickSound);
        optionButton.onClick.AddListener(OnOptionButtonClicked);
        optionButton.onClick.AddListener(AudioM.Instance.PlayButtonClickSound);
        stageSelectButton.onClick.AddListener(OnStageSelectButtonClicked);
        stageSelectButton.onClick.AddListener(AudioM.Instance.PlayButtonClickSound);
        exitButton.onClick.AddListener(OnExitButtonClicked);
        exitButton.onClick.AddListener(AudioM.Instance.PlayButtonClickSound);

        //オプション
        backPauseButton.onClick.AddListener(OnBackPauseButtonClicked);
        backPauseButton.onClick.AddListener(AudioM.Instance.PlayButtonClickSound);
        soundButton.onClick.AddListener(OnSoundButtonClicked);
        soundButton.onClick.AddListener(AudioM.Instance.PlayButtonClickSound);
        operateButton.onClick.AddListener(OnOperateButtonClicked);
        operateButton.onClick.AddListener(AudioM.Instance.PlayButtonClickSound);

        //サウンド
        backOpsionButton_1.onClick.AddListener(OnBackOpsionButtonClicked);
        backOpsionButton_1.onClick.AddListener(AudioM.Instance.PlayButtonClickSound);
        BGMSlider.onValueChanged.AddListener(value => AudioM.Instance.SetBackgroundMusicVolume(value));
        SESlider.onValueChanged.AddListener(value => AudioM.Instance.SetEffectsVolume(value));

        //操作設定
        backOpsionButton_2.onClick.AddListener(OnBackOpsionButton2Clicked);
        backOpsionButton_2.onClick.AddListener(AudioM.Instance.PlayButtonClickSound);
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
            if (isPaused && !isOptionMenu) { }
            else if (isPaused && isOptionMenu && !isSoundMenu) { }
            else if (isPaused && isOptionMenu && isSoundMenu && !isOperationMenu) { }
            else { TogglePause(); }
        }
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
        if (pauseMenuText != null)
        {
            isPaused = pauseMenuText.activeSelf;
            pauseMenuText.SetActive(!isPaused);
            resumeButton.gameObject.SetActive(!isPaused);
            restartButton.gameObject.SetActive(!isPaused);
            optionButton.gameObject.SetActive(!isPaused);
            stageSelectButton.gameObject.SetActive(!isPaused);
            exitButton.gameObject.SetActive(!isPaused);
            optionMenuText.SetActive(false);

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
        if (optionMenuText != null)
        {
            isOptionMenu = optionMenuText.activeSelf;
            optionMenuText.SetActive(!isOptionMenu);
            backPauseButton.gameObject.SetActive(!isOptionMenu);
            soundButton.gameObject.SetActive(!isOptionMenu);
            operateButton.gameObject.SetActive(!isOptionMenu);

            foreach (GameObject image in opsionImageObjects)
            {
                image.SetActive(!isOptionMenu);
            }

            Time.timeScale = isOptionMenu ? 0f : 0f;

            if (!isOptionMenu && EventSystem.current != null)
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
        Sound();
    }
    private void Sound()
    {
        if (soundMenuText != null)
        {
            isSoundMenu = soundMenuText.activeSelf;
            soundMenuText.SetActive(!isSoundMenu);

            backOpsionButton_1.gameObject.SetActive(!isSoundMenu);
            BGMSlider.gameObject.SetActive(!isSoundMenu);
            SESlider.gameObject.SetActive(!isSoundMenu);

            foreach (GameObject image in soundImageObjects)
            {
                image.SetActive(!isSoundMenu);
            }

            if (!isSoundMenu && EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(backOpsionButton_1.gameObject);
            }
        }
    }
    private void OnBackOpsionButtonClicked()
    {
        Sound();
        Opsion();
    }

    private void OnOperateButtonClicked()
    {
        Operate();
        Opsion();
    }
    private void Operate()
    {
        isOperationMenu = operationMenuText.activeSelf;
        operationMenuText.SetActive(!isOperationMenu);

        backOpsionButton_2.gameObject.SetActive(!isOperationMenu);

        foreach (GameObject image in operateImageObject)
        {
            image.SetActive(!isOperationMenu);
        }

        if (!isOperationMenu && EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(backOpsionButton_2.gameObject);
        }
    }
    private void OnBackOpsionButton2Clicked()
    {
        Operate();
        Opsion();
    }

    public void OnStageCleared()
    {
        PlayerPrefs.SetInt("Stage_" + currentStageIndex, 1);
        PlayerPrefs.Save();
    }
}
