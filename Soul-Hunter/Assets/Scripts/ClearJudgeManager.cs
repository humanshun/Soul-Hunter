using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClearJudgeManager : MonoBehaviour
{
    // 勝利テキストを表示するためのGameObject
    [SerializeField] private GameObject WinText;

    // ゴールを表示するためのGameObject
    [SerializeField] private GameObject Goal;

    // スタート時に勝利テキストとゴールを非表示にする
    void Start()
    {
        WinText.SetActive(false);
        Goal.SetActive(false);
    }

    // ボスが倒された時に呼び出されるメソッド
    public void OnBossDefeated()
    {
        if (WinText != null && Goal != null)
        {
            // 勝利テキストとゴールを表示する
            WinText.SetActive(true);
            Goal.SetActive(true);
        }
    }
}
