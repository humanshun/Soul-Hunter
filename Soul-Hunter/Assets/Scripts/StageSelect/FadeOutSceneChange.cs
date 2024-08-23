using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutSceneChange : MonoBehaviour
{
    public Image fadeImage; // フェードアウト用のImageコンポーネント
    public float fadeDuration = 1f; // フェードの時間

    public void FadeOutAndChangeScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    private IEnumerator FadeOut(string sceneName)
    {
        Color color = fadeImage.color;
        float elapsedTime = 0f;

        // フェードアウトを開始
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;

        // シーンをロード
        SceneManager.LoadScene(sceneName);
    }
}
