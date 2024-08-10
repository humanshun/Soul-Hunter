using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClearJugeManager : MonoBehaviour
{
    [SerializeField] private GameObject WinText;
    [SerializeField] private GameObject Goal;

    void Start()
    {
        WinText.SetActive(false);
        Goal.SetActive(false);
    }
    public void OnBossDefeated()
    {
        WinText.SetActive(true);
        Goal.SetActive(true);
    }
}
