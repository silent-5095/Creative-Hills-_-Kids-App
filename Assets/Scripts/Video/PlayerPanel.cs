using System;
using System.Collections;
using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

// ReSharper disable FieldCanBeMadeReadOnly.Local

namespace Video
{
    public class PlayerPanel : MonoBehaviour
    {
        // public static PlayerPanel Instance;
        // [SerializeField] private VideoPlayer vPlayer;
        [SerializeField] private VideoButton[] buttons;
        [SerializeField] private VideoPlayer player;

        [SerializeField] private GameObject fullScreen;

        // [SerializeField] private Slider[] timeLines;
        [SerializeField] private Slider[] soundSlider;
        [SerializeField] private AudioSource source;
        [SerializeField] private Image[] soundButton, playButton;
        [SerializeField] private Sprite pauseSp, playSp, muteSp, volumeSp;

        private void Start()
        {
            foreach (var slider in soundSlider)
            {
                slider.value = source.volume;
            }

            // soundSlider.value = source.volume;
            // Instance = this;
            foreach (var button in buttons)
            {
                button.SetVideoToPlayer(player);
                button.VideoButtonClickEvent += OnVideoButtonClickEvent;
            }
        }

        private void OnVideoButtonClickEvent(VideoClip clip)
        {
            player.Play();
            foreach (var pButton in playButton)
            {
                pButton.sprite = player.isPaused ? playSp : pauseSp;
            }

            source.mute = false;
            foreach (var sButton in soundButton)
            {
                sButton.sprite = volumeSp;
            }

            // soundButton.sprite = volumeSp;
            foreach (var slider in soundSlider)
            {
                slider.interactable = true;
            }

            // soundSlider.interactable = true;
        }

        // private void Update()
        // {
        //     // if (!_scrubTouched)
        //     //     foreach (var timeLine in timeLines)
        //     //     {
        //     //         timeLine.value = (vPlayer.frame / (float) vPlayer.frameCount);
        //     //     }
        //     // else
        //     //     vPlayer.time = _touchedSlider.value;
        // }


        public void OnStopButton()
        {
            player.Stop();
            foreach (var pb in playButton)
            {
                pb.sprite = playSp;
            }

            // playButton.sprite = playSp;
        }

        public void OnPauseButton()
        {
            player.Pause();
            foreach (var pb in playButton)
            {
                pb.sprite = playSp;
            }

            // playButton.sprite = playSp;
        }

        public void OnPlayButton()
        {
            if (player.isPaused)
                player.Play();
            else
                player.Pause();
            foreach (var pb in playButton)
            {
                pb.sprite = player.isPaused ? playSp : pauseSp;
            }
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
            player.Stop();
            player.Play();
            foreach (var pb in playButton)
            {
                pb.sprite = pauseSp;
            }
        }

        public void OnSoundButton()
        {
            var mute = source.mute;
            mute = !mute;
            source.mute = mute;
            foreach (var sb in soundButton)
            {
                sb.sprite = mute ? muteSp : volumeSp;
            }

            foreach (var slider in soundSlider)
            {
                slider.interactable = !mute;
            }
        }

        // private bool _scrubTouched;
        // private Slider _touchedSlider;

        // public void OnScrubSlider(Slider slider)
        // {
        //     if (_touchedSlider == slider)
        //     {
        //         _vPlayerTempTime = slider.value;
        //     }
        //
        //     // vPlayer.time = slider.value;
        //     // foreach (var timeLine in timeLines)
        //     // {
        //     //     if (timeLine == slider)
        //     //         return;
        //     //     timeLine.value = slider.value;
        //     // }
        // }
        //
        // public void OnPointerDownOnVideoSlider(Slider slider)
        // {
        //     _scrubTouched = true;
        //     _touchedSlider = slider;
        // }
        //
        // public void OnPointerUpOnVideoSlider(Slider slider)
        // {
        //     // foreach (var timeLine in timeLines)
        //     // {
        //     //     timeLine.value = (float) vPlayer.time;
        //     // }
        //
        //     StartCoroutine(Delay());
        // }
        //
        // private double _vPlayerTempTime;
        //
        // private IEnumerator Delay()
        // {
        //     yield return new WaitForSecondsRealtime(0.01f);
        //     vPlayer.time = _vPlayerTempTime;
        //     foreach (var timeLine in timeLines)
        //     {
        //         Debug.Log(_vPlayerTempTime);
        //         timeLine.value = (float) _vPlayerTempTime;
        //     }
        //
        //     yield return new WaitForSecondsRealtime(0.01f);
        //     _scrubTouched = false;
        //     _touchedSlider = null;
        // }

        public void OnSoundSlider(Slider slider)
        {
            source.volume = slider.value;
        }
    }
}