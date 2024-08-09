using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClearJugeManager : MonoBehaviour
{
    [SerializeField] private GameObject WinText;

    void Start()
    {
        WinText.SetActive(false);
    }
    public void OnBossDefeated()
    {
        WinText.SetActive(true);
    }
}
