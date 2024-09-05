using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutSceneChange : MonoBehaviour
{
    public Image fadeImage;        // フェードアウト用のImageコンポーネント
    public float fadeDuration = 1f; // フェードアウトにかかる時間

    // シーンをフェードアウトさせて変更するメソッド
    public void FadeOutAndChangeScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName)); // コルーチンでフェードアウトを実行
    }

    private IEnumerator FadeOut(string sceneName)
    {
        Color color = fadeImage.color; // Imageコンポーネントの色を取得
        float elapsedTime = 0f; // 経過時間を初期化

        // フェードアウトを開始
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime; // 経過時間を更新
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration); // 透明度を計算
            fadeImage.color = color; // 色を更新
            yield return null; // 次のフレームまで待つ
        }

        color.a = 1f; // 完全に不透明に設定
        fadeImage.color = color;

        // シーンをロード
        SceneManager.LoadScene(sceneName);
    }
}
