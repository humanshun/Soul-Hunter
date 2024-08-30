using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text startText;
    public float titleFadeDuration = 2.0f;  // タイトルのフェードイン時間
    public float blinkInterval = 1.0f;      // フェードイン・フェードアウトの時間
    public Button newGameButton;
    public Button continueButton;
    public Image fadeOutImage;               // フェードアウト用のイメージ
    public float fadeOutDuration = 1.0f;    // フェードアウト時間

    public AudioClip jumpSound;  // 効果音のクリップ
    public AudioClip attackSound;

    public AudioClip enemyJumpSound;
    public AudioClip enemyDeathSound;
    public AudioClip buttonSelectSound;  // ボタン選択時の音
    private AudioSource audioSource;  // AudioSourceコンポーネントへの参照

    private GameObject lastSelected;   // 最後に選択されたオブジェクトの参照

    private bool canStartGame = false;      // ゲーム開始可能かどうかのフラグ
    private bool isMenuVisible = false;     // メニューが表示されているかどうかのフラグ

    void Start()
    {
        // AudioSourceコンポーネントを取得
        audioSource = GetComponent<AudioSource>();

        // 初期設定で非表示に
        startText.gameObject.SetActive(false);
        newGameButton.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);

        // フェードアウトイメージを初期設定で非表示に
        fadeOutImage.gameObject.SetActive(false);

        // タイトルテキストのアルファ値を0に設定（透明）
        Color titleColor = titleText.color;
        titleColor.a = 0;
        titleText.color = titleColor;

        // コルーチンを開始してフェードインと「push space to start」を表示
        StartCoroutine(FadeInTitle());

        // コルーチンを開始して音を再生する
        StartCoroutine(PlayerSound());
        StartCoroutine(EnemySound());

        // ボタンのクリックイベントにメソッドを追加
        newGameButton.onClick.AddListener(() => StartCoroutine(NewGame()));
        newGameButton.onClick.AddListener(AudioM.Instance.PlayButtonClickSound);
        continueButton.onClick.AddListener(() => StartCoroutine(FadeOutAndLoadScene("StageSelect")));
        continueButton.onClick.AddListener(AudioM.Instance.PlayButtonClickSound);
    }

    void Update()
    {
        // Spaceキーが押されたときにメニューを表示
        if (canStartGame && !isMenuVisible && Input.GetKeyDown(KeyCode.Space))
        {
            startText.gameObject.SetActive(false);
            newGameButton.gameObject.SetActive(true);
            continueButton.gameObject.SetActive(true);

            // 最初のボタンにフォーカスを設定
            EventSystem.current.SetSelectedGameObject(newGameButton.gameObject);

            // メニューが表示されたことを記録
            isMenuVisible = true;
        }

        // ボタン選択変更時に音を再生
        HandleButtonSelectionChange();

        // WSキーでもボタン間を移動可能に
        if (isMenuVisible && EventSystem.current.currentSelectedGameObject != null)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                Selectable previous = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
                if (previous != null)
                {
                    previous.Select();
                }
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                Selectable next = EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
                if (next != null)
                {
                    next.Select();
                }
            }
        }
    }

    private void HandleButtonSelectionChange()
    {
        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

        if (currentSelected != null && currentSelected != lastSelected)
        {
            if (currentSelected.GetComponent<Button>() != null)
            {
                audioSource.PlayOneShot(buttonSelectSound);
                lastSelected = currentSelected;
            }
        }
    }

    IEnumerator FadeInTitle()
    {
        yield return new WaitForSeconds(5f);
        // タイトルテキストを徐々に表示
        float elapsedTime = 0f;

        while (elapsedTime < titleFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / titleFadeDuration);
            Color titleColor = titleText.color;
            titleColor.a = alpha;
            titleText.color = titleColor;

            yield return null;
        }

        // タイトル表示完了後、「push space to start」をフェードイン・フェードアウトで点滅
        StartCoroutine(BlinkStartText());

        // ゲーム開始可能フラグをtrueに設定
        canStartGame = true;
    }

    IEnumerator PlayerSound()
    {
        JumpSound();
        yield return new WaitForSeconds(1.25f);
        JumpSound();
        yield return new WaitForSeconds(1.20f);
        JumpSound();
        yield return new WaitForSeconds(0.55f);
        AttackSound();
    }
    private void JumpSound()
    {
        audioSource.PlayOneShot(jumpSound);
    }
    private void AttackSound()
    {
        audioSource.PlayOneShot(attackSound);
    }

    IEnumerator EnemySound()
    {
        EnemyJumpSound();
        yield return new WaitForSeconds(1.4f);
        EnemyJumpSound();
        yield return new WaitForSeconds(3f);
        audioSource.PlayOneShot(enemyDeathSound);
    }
    private void EnemyJumpSound()
    {
        audioSource.PlayOneShot(enemyJumpSound);
    }

    IEnumerator BlinkStartText()
    {
        // 「push space to start」を表示
        startText.gameObject.SetActive(true);

        while (true)
        {
            // フェードイン
            yield return StartCoroutine(FadeTextToFullAlpha(blinkInterval, startText));

            // フェードアウト
            yield return StartCoroutine(FadeTextToZeroAlpha(blinkInterval, startText));
        }
    }

    IEnumerator FadeTextToFullAlpha(float duration, TMP_Text text)
    {
        Color color = text.color;
        for (float t = 0.01f; t < duration; t += Time.deltaTime)
        {
            color.a = Mathf.Lerp(0, 1, t / duration);
            text.color = color;
            yield return null;
        }
        color.a = 1;
        text.color = color;
    }

    IEnumerator FadeTextToZeroAlpha(float duration, TMP_Text text)
    {
        Color color = text.color;
        for (float t = 0.01f; t < duration; t += Time.deltaTime)
        {
            color.a = Mathf.Lerp(1, 0, t / duration);
            text.color = color;
            yield return null;
        }
        color.a = 0;
        text.color = color;
    }

    IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        // フェードアウトイメージを表示
        fadeOutImage.gameObject.SetActive(true);

        // フェードアウトを実行
        Color imageColor = fadeOutImage.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeOutDuration);
            imageColor.a = alpha;
            fadeOutImage.color = imageColor;
            yield return null;
        }

        // 完全にフェードアウトした後にシーン遷移
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator NewGame()
    {
        // ステージ状況をリセット
        ResetAllStages();

        //キャラクター能力をリセット
        PlayerMovement.jumpAbilityFlag = false;
        PlayerMovement.slashAbilityFlag = false;
        PlayerMovement.shootAbilityFlag = false;


        // シーン遷移
        yield return StartCoroutine(FadeOutAndLoadScene("StageSelect"));
    }

    void ResetAllStages()
    {
        // すべてのステージクリア状況をリセット
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
