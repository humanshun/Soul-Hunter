using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReturnToTitle : MonoBehaviour
{
    public Button returnButton; // ボタンの参照

    void Start()
    {
        // ボタンを選択状態にする
        returnButton.Select();

        // ボタンにクリックイベントを追加
        returnButton.onClick.AddListener(ReturnToTitleScreen);
    }

    void Update()
    {
        // スペースキーが押されたとき
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ボタンを押す
            returnButton.onClick.Invoke();
        }
    }

    // タイトル画面に戻る処理
    void ReturnToTitleScreen()
    {
        SceneManager.LoadScene("Title");
    }
}
