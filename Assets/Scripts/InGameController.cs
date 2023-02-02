using System;
using System.Collections;
using System.Collections.Generic;
using GameScene;
using Painting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameController : MonoBehaviour
{
    public static InGameController Instance;
    [SerializeField] private bool loadFromIslands;
    [SerializeField] private string defaultSceneName;
    [SerializeField] private GameObject winPanel;

    private void Start()
    {
        if (Instance is not null)
            Destroy(Instance);
        Instance = this;
        // loadFromIslands = !string.IsNullOrEmpty(Island.IslandGameRef);
    }

    public void OnSubmitButtonClick()
    {
        ForDemo.Instance.LoadScene(defaultSceneName);
        // SceneManager.LoadScene(loadFromIslands ? "Island" : defaultSceneName);
    }

    public void OnBackButtonClick()
    {
        Island.IslandGameRef = string.Empty;
        OnSubmitButtonClick();
    }

    public void Win()
    {
        if (loadFromIslands)
        {
            OnSubmitButtonClick();
        }
        else
            winPanel.GetComponent<WinPanel>().ShowWinPanel();
    }

    public void Exit()
    {
    }
}