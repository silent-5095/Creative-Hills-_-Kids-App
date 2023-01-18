using System;
using System.Collections;
using System.Collections.Generic;
using GameScene;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameController : MonoBehaviour
{
    public static InGameController Instance;
    [SerializeField] private bool loadFromIslands;
    [SerializeField] private string defaultSceneName;
    [SerializeField] private GameObject winPanel, exitPanel;
    [SerializeField] private Button submitButton, cancelButton;

    private void Start()
    {
        if (Instance is not null)
            Destroy(Instance);
        Instance = this;
        loadFromIslands = !string.IsNullOrEmpty(Island.IslandGameRef);
    }

    public void OnSubmitButtonClick()
    {
        SceneManager.LoadScene(loadFromIslands ? "Island" : defaultSceneName);
    }

    public void Win()
    {
        if (loadFromIslands)
        {
            PlayerPrefs.SetString("IslandGameRef" + Island.IslandGameRef, "1");
            Island.IslandGameRef = string.Empty;
        }

        winPanel.SetActive(true);
    }

    public void Exit()
    {
    }
}