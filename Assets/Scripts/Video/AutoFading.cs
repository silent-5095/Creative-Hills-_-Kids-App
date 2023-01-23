using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class AutoFading : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private VideoPlayer vPlayer;

    [SerializeField] private float delay = 0,touchTime=1;
    private float _timer = 0,_touchDuration;
    private bool _inFading;

    private void Start()
    {
        canvasGroup.alpha = 1;
    }

    private void Update()
    {
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                touchTime = 0;
            }
            else if (touch.phase==TouchPhase.Moved || touch.phase== TouchPhase.Moved)
            {
                _touchDuration += Time.deltaTime;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if (_touchDuration<=touchTime)
                {
                    canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
                }

                _touchDuration = 0;
            }
            _inFading = false;
            _timer = 0;
        }

        if (vPlayer.isPaused)
        {
            canvasGroup.alpha = 1;
            return;
        }

        _timer += Time.deltaTime;
        if (!(_timer >= delay) || _inFading) return;
        _inFading = true;
        AutoFade();
    }

    private void AutoFade()
    {
        var tween = canvasGroup.DOFade(0, 0.5f);
        tween.onComplete += () => { tween.Kill(); };
    }
}