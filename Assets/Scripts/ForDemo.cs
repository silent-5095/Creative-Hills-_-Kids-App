using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForDemo : MonoBehaviour
{
    public static ForDemo Instance;
    [SerializeField] private ScreenOrientation orientation=ScreenOrientation.LandscapeLeft;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Screen.orientation = orientation;
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
