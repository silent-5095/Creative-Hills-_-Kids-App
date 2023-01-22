using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.U2D;

namespace SlidingPipes
{
    public class Path : MonoBehaviour
    {
        public static event Action EndAnimationEvent;
        [SerializeField] private SpriteShapeController path;
        [SerializeField] private Transform ball;
        [SerializeField] private int index;
        [SerializeField] private bool update;

        private void OnDestroy()
        {
            Matrix.WinEvent -= SectionOnMoveEvent;
            AspectDetector.AspectDetectorEvent -= OnAspectDetectorEvent;
        }

        private void Awake()
        {
            Matrix.WinEvent+= SectionOnMoveEvent;
            AspectDetector.AspectDetectorEvent += OnAspectDetectorEvent;
        }

        private void Start()
        {
            ball.localPosition = path.spline.GetPosition(0);
            index = 0;
            StartCoroutine(Move(index));
        }

        private void SectionOnMoveEvent(bool con)
        {
            update = true;
        }

        private IEnumerator Move(int id)
        {
            yield return new WaitForSeconds(0.01f);
            if (update)
            {
                if (id >= path.spline.GetPointCount())
                {
                    EndAnimationEvent?.Invoke();
                    yield break;
                    // id = index;
                    // ball.localPosition = path.spline.GetPosition(0);
                }
                var tween = ball.DOLocalMove(path.spline.GetPosition(id), 0.2f);
                tween.SetEase(Ease.Linear);
                tween.onComplete += () =>
                {
                    StartCoroutine(Move(id + 1));
                    tween.Kill();
                };
            }
            else
                StartCoroutine(Move(id));
        }private void OnAspectDetectorEvent(bool isTablet)
        {
            // if (isTablet)
            // {
            //     transform.localScale = transform.localScale * 0.8f;
            // }
            // else
            //     transform.localScale = transform.localScale;

            
        }
    }
}