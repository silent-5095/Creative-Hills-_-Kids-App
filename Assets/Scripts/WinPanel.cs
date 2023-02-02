using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPanel : MonoBehaviour
{
    [SerializeField] internal GameObject mainPanel;
    [SerializeField] internal GameObject[] panels;
    [SerializeField] internal string winPanel="WinPanel";
    internal int CurrentIndex;

    internal virtual void Start()
    {
        CurrentIndex = PlayerPrefs.GetInt(winPanel, 0);
    }

    public virtual void ShowWinPanel()
    {
        mainPanel.SetActive(true);
        panels[CurrentIndex].SetActive(true);
        CurrentIndex = CurrentIndex < panels.Length-1 ? CurrentIndex + 1 : 0;
        Debug.Log(CurrentIndex);
        PlayerPrefs.SetInt(winPanel, CurrentIndex);
    }
}