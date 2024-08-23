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

    private bool canStartGame = false;      // ゲーム開始可能かどうかのフラグ
    private bool isMenuVisible = false;     // メニューが表示されているかどうかのフラグ

    void Start()
    {
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

        // ボタンのクリックイベントにメソッドを追加
        newGameButton.onClick.AddListener(() => StartCoroutine(FadeOutAndLoadScene("StageSelect")));
        continueButton.onClick.AddListener(() => StartCoroutine(FadeOutAndLoadScene("StageSelect")));
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
}
