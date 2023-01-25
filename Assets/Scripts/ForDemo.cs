using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForDemo : MonoBehaviour
{
    public static ForDemo Instance;
    [SerializeField] private ScreenOrientation orientation;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Screen.orientation = orientation;
        Screen.autorotateToPortrait = true;
        Debug.Log(Screen.orientation);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
