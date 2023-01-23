using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

namespace Video
{
    public class TimeScrub : MonoBehaviour
    {
        [SerializeField] public VideoPlayer vPlayer;

        // [SerializeField] private Text currentTime, totalTime;
        private bool _setUp, _videoPlayerRide = true;

        private void SetUp(VideoPlayer videoPlayer)
        {
            if (videoPlayer is null)
            {
                visualSlider.value = 0;
                touchSlider.value = 0;
                _setUp = false;
                return;
            }

            visualSlider.value = 0;
            touchSlider.value = 0;
            var maxVal = Mathf.Round((float) vPlayer.length);
            visualSlider.maxValue = maxVal;
            touchSlider.maxValue = maxVal;
            // totalTime.text = TimerFormatHandler((int) vPlayer.length);
            _setUp = true;
        }


        [SerializeField] private CustomSlider visualSlider;
        [SerializeField] private CustomSlider touchSlider;

        private void Start()
        {
            vPlayer.loopPointReached += source =>
            {
                visualSlider.value = 0;
                touchSlider.value = 0;
            };
            vPlayer.prepareCompleted += SetUp;
            touchSlider.PointerDownEvent += () =>
            {
                _videoPlayerRide = false;
                touchSlider.SetValue((float) vPlayer.time);
                vPlayer.Pause();
                StopAllCoroutines();
                StartCoroutine(ToucheSliderRide());
            };
            touchSlider.PointerUpEvent += () =>
            {
                StopAllCoroutines();
                visualSlider.SetValue(touchSlider.value);
                vPlayer.time = touchSlider.value;
                vPlayer.Play();
                StartCoroutine(CheckVSlider());
            };
        }

        private IEnumerator CheckVSlider()
        {
            var value = touchSlider.value;
            vPlayer.time = value;
            vPlayer.Play();
            visualSlider.SetValue(value);
            yield return new WaitWhile(() => Math.Abs((float) vPlayer.time - touchSlider.value) > 0.5f);
            _videoPlayerRide = true;
        }

        private IEnumerator ToucheSliderRide()
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            visualSlider.SetValue(touchSlider.value);
            StartCoroutine(ToucheSliderRide());
        }
        private void FixedUpdate()
        {
            if (!_setUp)
            {
                SetUp(vPlayer);
                return;
            }

            // currentTime.text = TimerFormatHandler((int) vPlayer.time);
            if (vPlayer is null || !_videoPlayerRide)
                return;
            visualSlider.SetValue((float) vPlayer.time);
        }

        private string TimerFormatHandler(int time)
        {
            var result = string.Empty;
            var remain = time;
            var hours = remain / 3600;
            result += hours >= 10 ? hours + ":" : "0" + hours + ":";
            remain -= (hours * 3600);
            var minutes = remain / 60;
            result += minutes >= 10 ? minutes + ":" : "0" + minutes + ":";
            remain -= minutes * 60;
            var seconds = remain;
            result += seconds >= 10 ? seconds + "" : "0" + seconds;
            return result;
        }
    }
}