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
        [SerializeField] private VideoPlayer vPlayer;
        [SerializeField] private VideoButton[] buttons;
        [SerializeField] private VideoPlayer player;
        [SerializeField] private GameObject fullScreen;
        // [SerializeField] private Slider[] timeLines;
        [SerializeField] private Slider soundSlider;
        [SerializeField] private AudioSource source;
        [SerializeField] private Image soundButton, playButton;
        [SerializeField] private Sprite pauseSp, playSp, muteSp, volumeSp;

        private void Start()
        {
            soundSlider.value = source.volume;
            // Instance = this;
            foreach (var button in buttons)
            {
                button.SetVideoToPlayer(player);
                button.VideoButtonClickEvent += OnVideoButtonClickEvent;
            }
        }

        private void OnVideoButtonClickEvent(VideoClip clip)
        {
            vPlayer.Play();
            playButton.sprite = vPlayer.isPaused ? playSp : pauseSp;

            source.mute = false;
            soundButton.sprite = volumeSp;
            soundSlider.interactable = true;
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
            vPlayer.Stop();
            playButton.sprite = playSp;
        }

        public void OnPauseButton()
        {
            vPlayer.Pause();
            playButton.sprite = playSp;
        }

        public void OnPlayButton()
        {
            if (vPlayer.isPaused)
                vPlayer.Play();
            else
                vPlayer.Pause();
            playButton.sprite = vPlayer.isPaused ? playSp : pauseSp;
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
            playButton.sprite = pauseSp;
        }

        public void OnSoundButton()
        {
            var mute = source.mute;
            mute = !mute;
            source.mute = mute;
            soundButton.sprite = mute ? muteSp : volumeSp;
            soundSlider.interactable = !mute;
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

        public void OnSoundSlider()
        {
            source.volume = soundSlider.value;
        }
    }
}