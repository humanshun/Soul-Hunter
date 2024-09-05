using UnityEngine;
using UnityEngine.UI;
using TMPro;  // TextMeshProを使うために追加
using System.Collections;  // IEnumeratorのために追加

public class FadeInController : MonoBehaviour
{
    public Image fadeImage;               // フェードインに使用する画像
    public TextMeshProUGUI fadeText;      // フェードインに使用するTextMeshProUGUI
    public float fadeDuration = 2f;       // フェードインにかかる時間
    public float initialDelay = 1f;       // 最初の黒い画面を表示する時間

    private void Start()
    {
        // 最初に黒い画面に設定
        Color color = fadeImage.color;
        color.a = 1f;  // 画面を完全に黒にする
        fadeImage.color = color;

        if (fadeText != null)
        {
            Color textColor = fadeText.color;
            textColor.a = 1f;  // テキストを最初に表示させておく
            fadeText.color = textColor;
        }

        // ゲームの時間を停止
        Time.timeScale = 0f;

        // フェードインを開始
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        // 最初の1秒間、黒い画面を表示
        yield return new WaitForSecondsRealtime(initialDelay);

        float elapsedTime = 0f;
        Color imageColor = fadeImage.color;
        Color textColor = fadeText.color;

        // フェードイン処理
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;  // フェードイン処理の進行をゲームの時間に依存しないようにする
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);

            imageColor.a = 1f - alpha;  // 画像の透明度を減少させる
            fadeImage.color = imageColor;

            if (fadeText != null)
            {
                textColor.a = Mathf.Clamp01(1f - alpha);  // テキストの透明度を変更
                fadeText.color = textColor;
            }

            yield return null;
        }

        // フェードインが終了
        imageColor.a = 0f;  // 画像の透明度を完全に0にする
        fadeImage.color = imageColor;

        if (fadeText != null)
        {
            textColor.a = 0f;  // テキストの透明度を完全に0にする
            fadeText.color = textColor;
        }

        // ゲームの時間を再開
        Time.timeScale = 1f;
    }
}
