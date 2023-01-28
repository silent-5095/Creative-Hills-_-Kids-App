using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Video
{
    public class VideoButton : MonoBehaviour
    {
        public event Action<VideoClip> VideoButtonClickEvent;
        [SerializeField] private Button button;
        [SerializeField] private VideoClip clip;
        private VideoPlayer _videoPlayer;

        private void Start()
        {
            button.onClick.AddListener(OnButtonClick);
        }

        public void SetVideoToPlayer(VideoPlayer player)
        {
            _videoPlayer = player;
            _videoPlayer.prepareCompleted += OnPrepareVideo;
        }

        private void OnPrepareVideo(VideoPlayer vp)
        {
            vp.Play();
        }

        private void OnButtonClick()
        {
            _videoPlayer.clip = clip;
            _videoPlayer.Prepare();
            VideoButtonClickEvent?.Invoke(clip);
        }
    }
}



