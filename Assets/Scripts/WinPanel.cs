using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPanel : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject[] panels;
    private int _currentIndex;

    private void Start()
    {
        _currentIndex = PlayerPrefs.GetInt("WinPanel", 0);
    }

    public void ShowWinPanel()
    {
        mainPanel.SetActive(true);
        panels[_currentIndex].SetActive(true);
        _currentIndex = _currentIndex < panels.Length-1 ? _currentIndex + 1 : 0;
        PlayerPrefs.SetInt("WinPanel", _currentIndex);
    }
}