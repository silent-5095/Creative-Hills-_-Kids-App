using System;
using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Video
{
    public class PlayerPanel : MonoBehaviour
    {
        public static PlayerPanel Instance;
        [SerializeField] private VideoPlayer vPlayer;
        [SerializeField] private VideoButton[] buttons;
        [SerializeField] private VideoPlayer player;
        [SerializeField] private GameObject fullScreen;
        [SerializeField] private Scrollbar[] timeLines;

        private void Start()
        {
            Instance = this;
            foreach (var button in buttons)
            {
                button.SetVideoToPlayer(player);
                button.VideoButtonClickEvent += OnVideoButtonClickEvent;
            }
        }

        private void OnVideoButtonClickEvent(VideoClip clip)
        {
        }

        private void Update()
        {
            foreach (var timeLine in timeLines)
            {
                timeLine.value = (vPlayer.frame / (float) vPlayer.frameCount);
            }
        }


        public void OnStopButton()
        {
            vPlayer.Stop();
        }

        public void OnPauseButton()
        {
            vPlayer.Pause();
        }

        public void OnPlayButton()
        {
            vPlayer.Play();
        }

        public void OnNextButton()
        {
        }

        public void OnFullScreenButton()
        {
            fullScreen.SetActive(true);
        }

        public void OnReplayButton()
        {
            vPlayer.Stop();
            vPlayer.Play();
        }
    }
}