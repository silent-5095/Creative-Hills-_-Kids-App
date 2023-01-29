using UnityEngine;
using UnityEngine.Video;

public class Splash : MonoBehaviour
{
    [SerializeField] private VideoPlayer player;

    private void Awake()
    {
        player.loopPointReached+= PlayerEndloopPointReached;
    }

    private void PlayerEndloopPointReached(VideoPlayer source)
    {
        ForDemo.Instance.LoadScene("FirstScene");
    }
}
