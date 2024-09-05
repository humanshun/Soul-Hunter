using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameM : MonoBehaviour
{
    [SerializeField] private static int life = 3; // プレイヤーのライフ数を指定します
    [SerializeField] private string gameOverScene = "GameOver"; // ゲームオーバーシーンの名前を指定します
    [SerializeField] private int currentStageIndex; // 現在のステージインデックスを指定します
    private bool isPaused; // ゲームがポーズ中かどうかを管理します
    private bool isOptionMenu; // オプションメニューが開かれているかどうかを管理します
    private bool isSoundMenu; // サウンドメニューが開かれているかどうかを管理します
    private bool isOperationMenu; // 操作方法メニューが開かれているかどうかを管理します
    [SerializeField] private GameObject pauseMenuText; // ポーズメニューのテキストオブジェクトを指定します
    [SerializeField] private Button resumeButton; // 再開ボタンを指定します
    [SerializeField] private Button restartButton; // 再スタートボタンを指定します
    [SerializeField] private Button optionButton; // オプションボタンを指定します
    [SerializeField] private Button stageSelectButton; // ステージ選択ボタンを指定します
    [SerializeField] private Button exitButton; // 終了ボタンを指定します
    [SerializeField] private GameObject[] pauseImageObject; // ポーズメニューの画像オブジェクトを配列で指定します

    [SerializeField] private GameObject optionMenuText; // オプションメニューのテキストオブジェクトを指定します
    [SerializeField] private Button backPauseButton; // ポーズに戻るボタンを指定します
    [SerializeField] private Button soundButton; // サウンドボタンを指定します
    [SerializeField] private Button operateButton; // 操作方法ボタンを指定します
    [SerializeField] private GameObject[] opsionImageObjects; // オプションメニューの画像オブジェクトを配列で指定します

    [SerializeField] private GameObject soundMenuText; // サウンドメニューのテキストオブジェクトを指定します
    [SerializeField] private Button backOpsionButton_1; // オプションに戻るボタンを指定します
    [SerializeField] private Slider BGMSlider; // BGM用のスライダーを指定します
    [SerializeField] private Slider SESlider; // 効果音用のスライダーを指定します
    [SerializeField] private GameObject[] soundImageObjects; // サウンドメニューの画像オブジェクトを配列で指定します

    [SerializeField] private GameObject operationMenuText; // 操作方法メニューのテキストオブジェクトを指定します
    [SerializeField] private Button backOpsionButton_2; // オプションに戻るボタンを指定します
    [SerializeField] private Image operationImage; // 操作方法の画像オブジェクトを指定します
    [SerializeField] private GameObject[] operateImageObject; // 操作方法メニューの画像オブジェクトを配列で指定します

    private GameObject lastSelected; // 最後に選択されたボタンオブジェクトを管理します

    public static GameM Instance { get; private set; } // シングルトンパターンのためのインスタンスを取得します

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 重複したインスタンスがあれば破棄します
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // シーンをロードしてもこのオブジェクトを破棄しないようにします
    }

    private void Start()
    {
        // スライダーの初期値設定
        BGMSlider.value = AudioM.Instance.BGMVolume; // BGMの音量初期値を設定します
        SESlider.value = AudioM.Instance.EffectsVolume; // 効果音の音量初期値を設定します

        isPaused = true; // 初期状態ではポーズ中として設定します
        isOptionMenu = true; // 初期状態ではオプションメニューが開かれていると設定します
        isSoundMenu = true; // 初期状態ではサウンドメニューが開かれていると設定します
        isOperationMenu = true; // 初期状態では操作方法メニューが開かれていると設定します

        // ポーズメニューを非表示に設定します
        pauseMenuText.SetActive(false);
        resumeButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        optionButton.gameObject.SetActive(false);
        stageSelectButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);

        foreach (GameObject image in pauseImageObject)
        {
            image.SetActive(false); // 各ポーズメニューの画像オブジェクトを非表示にします
        }

        // オプションメニューを非表示に設定します
        optionMenuText.SetActive(false);
        backPauseButton.gameObject.SetActive(false);
        soundButton.gameObject.SetActive(false);
        operateButton.gameObject.SetActive(false);

        foreach (GameObject image in opsionImageObjects)
        {
            image.SetActive(false); // 各オプションメニューの画像オブジェクトを非表示にします
        }

        // サウンドメニューを非表示に設定します
        soundMenuText.SetActive(false);
        backOpsionButton_1.gameObject.SetActive(false);
        BGMSlider.gameObject.SetActive(false);
        SESlider.gameObject.SetActive(false);

        foreach (GameObject image in soundImageObjects)
        {
            image.SetActive(false); // 各サウンドメニューの画像オブジェクトを非表示にします
        }

        // 操作方法メニューを非表示に設定します
        operationMenuText.SetActive(false);
        backOpsionButton_2.gameObject.SetActive(false);
        operationImage.gameObject.SetActive(false);

        foreach (GameObject image in operateImageObject)
        {
            image.SetActive(false); // 各操作方法メニューの画像オブジェクトを非表示にします
        }

        // ポーズメニューのボタンにクリックリスナーを追加します
        resumeButton.onClick.AddListener(OnResumeButtonClicked);
        resumeButton.onClick.AddListener(AudioM.Instance.PlayButtonClickSound); // クリック音を再生します
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        restartButton.onClick.AddListener(AudioM.Instance.PlayButtonClickSound); // クリック音を再生します
        optionButton.onClick.AddListener(OnOptionButtonClicked);
        optionButton.onClick.AddListener(AudioM.Instance.PlayButtonClickSound); // クリック音を再生します
        stageSelectButton.onClick.AddListener(OnStageSelectButtonClicked);
        stageSelectButton.onClick.AddListener(AudioM.Instance.PlayButtonClickSound); // クリック音を再生します
        exitButton.onClick.AddListener(OnExitButtonClicked);
        exitButton.onClick.AddListener(AudioM.Instance.PlayButtonClickSound); // クリック音を再生します

        // オプションメニューのボタンにクリックリスナーを追加します
        backPauseButton.onClick.AddListener(OnBackPauseButtonClicked);
        backPauseButton.onClick.AddListener(AudioM.Instance.PlayButtonClickSound); // クリック音を再生します
        soundButton.onClick.AddListener(OnSoundButtonClicked);
        soundButton.onClick.AddListener(AudioM.Instance.PlayButtonClickSound); // クリック音を再生します
        operateButton.onClick.AddListener(OnOperateButtonClicked);
        operateButton.onClick.AddListener(AudioM.Instance.PlayButtonClickSound); // クリック音を再生します

        // サウンドメニューのスライダーにリスナーを追加します
        backOpsionButton_1.onClick.AddListener(OnBackOpsionButtonClicked);
        backOpsionButton_1.onClick.AddListener(AudioM.Instance.PlayButtonClickSound); // クリック音を再生します
        BGMSlider.onValueChanged.AddListener(value => AudioM.Instance.SetBackgroundMusicVolume(value)); // BGM音量を設定します
        SESlider.onValueChanged.AddListener(value => AudioM.Instance.SetEffectsVolume(value)); // 効果音の音量を設定します

        // 操作方法メニューのボタンにクリックリスナーを追加します
        backOpsionButton_2.onClick.AddListener(OnBackOpsionButton2Clicked);
        backOpsionButton_2.onClick.AddListener(AudioM.Instance.PlayButtonClickSound); // クリック音を再生します
    }

    private void Update()
    {
        HandlePauseToggle(); // ポーズの切り替えを処理します
        HandleButtonSelectionChange(); // ボタン選択の変更を処理します
    }

    private void HandlePauseToggle()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused && !isOptionMenu) { }
            else if (isPaused && isOptionMenu && !isSoundMenu) { }
            else if (isPaused && isOptionMenu && isSoundMenu && !isOperationMenu) { }
            else { TogglePause(); } // ポーズのオン・オフを切り替えます
        }
    }

    private void HandleButtonSelectionChange()
    {
        GameObject currentSelected = EventSystem.current.currentSelectedGameObject; // 現在選択されているボタンを取得します

        if (currentSelected != null && currentSelected != lastSelected)
        {
            if (currentSelected.GetComponent<Button>() != null)
            {
                AudioM.Instance.PlayButtonSelectSound(); // ボタン選択音を再生します
                lastSelected = currentSelected; // 最後に選択されたボタンを更新します
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
                SceneManager.LoadScene(gameOverScene); // ライフが0以下になったらゲームオーバーシーンをロードします
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 現在のシーンを再ロードします
            }
        }
    }

    private void TogglePause()
    {
        if (pauseMenuText != null)
        {
            isPaused = pauseMenuText.activeSelf;
            pauseMenuText.SetActive(!isPaused); // ポーズメニューの表示/非表示を切り替えます
            resumeButton.gameObject.SetActive(!isPaused);
            restartButton.gameObject.SetActive(!isPaused);
            optionButton.gameObject.SetActive(!isPaused);
            stageSelectButton.gameObject.SetActive(!isPaused);
            exitButton.gameObject.SetActive(!isPaused);
            optionMenuText.SetActive(false);

            foreach (GameObject image in pauseImageObject)
            {
                image.SetActive(!isPaused); // 各ポーズメニューの画像オブジェクトの表示/非表示を切り替えます
            }

            Time.timeScale = isPaused ? 1f : 0f; // ゲームの時間の流れを制御します

            if (!isPaused && EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(resumeButton.gameObject); // 再開ボタンを選択状態にします
            }
        }
    }

    private void OnResumeButtonClicked()
    {
        TogglePause(); // ポーズメニューを閉じます
    }

    private void OnRestartButtonClicked()
    {
        TogglePause(); // ポーズメニューを閉じます
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 現在のシーンを再ロードします
    }

    private void OnOptionButtonClicked()
    {
        TogglePause(); // ポーズメニューを閉じます
        Opsion(); // オプションメニューを開きます
    }

    private void Opsion()
    {
        if (optionMenuText != null)
        {
            isOptionMenu = optionMenuText.activeSelf;
            optionMenuText.SetActive(!isOptionMenu); // オプションメニューの表示/非表示を切り替えます
            backPauseButton.gameObject.SetActive(!isOptionMenu);
            soundButton.gameObject.SetActive(!isOptionMenu);
            operateButton.gameObject.SetActive(!isOptionMenu);

            foreach (GameObject image in opsionImageObjects)
            {
                image.SetActive(!isOptionMenu); // 各オプションメニューの画像オブジェクトの表示/非表示を切り替えます
            }

            Time.timeScale = isOptionMenu ? 0f : 0f; // ゲームの時間の流れを制御します

            if (!isOptionMenu && EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(backPauseButton.gameObject); // ポーズに戻るボタンを選択状態にします
            }
        }
    }

    private void OnStageSelectButtonClicked()
    {
        TogglePause(); // ポーズメニューを閉じます
        SceneManager.LoadScene("StageSelect"); // ステージ選択シーンをロードします
    }

    private void OnExitButtonClicked()
    {
        TogglePause(); // ポーズメニューを閉じます
        SceneManager.LoadScene("Title"); // タイトルシーンをロードします
    }

    private void OnBackPauseButtonClicked()
    {
        Opsion(); // オプションメニューを閉じます
        TogglePause(); // ポーズメニューを閉じます
    }

    private void OnSoundButtonClicked()
    {
        Opsion(); // オプションメニューを閉じます
        Sound(); // サウンドメニューを開きます
    }
    private void Sound()
    {
        if (soundMenuText != null)
        {
            isSoundMenu = soundMenuText.activeSelf;
            soundMenuText.SetActive(!isSoundMenu); // サウンドメニューの表示/非表示を切り替えます

            backOpsionButton_1.gameObject.SetActive(!isSoundMenu);
            BGMSlider.gameObject.SetActive(!isSoundMenu);
            SESlider.gameObject.SetActive(!isSoundMenu);

            foreach (GameObject image in soundImageObjects)
            {
                image.SetActive(!isSoundMenu); // 各サウンドメニューの画像オブジェクトの表示/非表示を切り替えます
            }

            if (!isSoundMenu && EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(backOpsionButton_1.gameObject); // オプションに戻るボタンを選択状態にします
            }
        }
    }
    private void OnBackOpsionButtonClicked()
    {
        Sound(); // サウンドメニューを閉じます
        Opsion(); // オプションメニューを開きます
    }

    private void OnOperateButtonClicked()
    {
        Operate(); // 操作方法メニューを開きます
        Opsion(); // オプションメニューを閉じます
    }
    private void Operate()
    {
        isOperationMenu = operationMenuText.activeSelf;
        operationMenuText.SetActive(!isOperationMenu); // 操作方法メニューの表示/非表示を切り替えます

        backOpsionButton_2.gameObject.SetActive(!isOperationMenu);

        foreach (GameObject image in operateImageObject)
        {
            image.SetActive(!isOperationMenu); // 各操作方法メニューの画像オブジェクトの表示/非表示を切り替えます
        }

        if (!isOperationMenu && EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(backOpsionButton_2.gameObject); // オプションに戻るボタンを選択状態にします
        }
    }
    private void OnBackOpsionButton2Clicked()
    {
        Operate(); // 操作方法メニューを閉じます
        Opsion(); // オプションメニューを開きます
    }

    public void OnStageCleared()
    {
        PlayerPrefs.SetInt("Stage_" + currentStageIndex, 1); // クリアしたステージを保存します
        PlayerPrefs.Save(); // プレイヤーの進行状況を保存します
    }
}
