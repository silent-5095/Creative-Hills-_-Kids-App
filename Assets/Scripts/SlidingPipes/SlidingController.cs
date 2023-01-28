using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

namespace SlidingPipes
{
    public class SlidingController : MonoBehaviour
    {
        [SerializeField] private GameObject matrix3In3, matrix4In4, matrix5In5;
        [SerializeField] private MatrixType matrixType;
        [SerializeField] private SpriteRenderer lastMsgSprite;
        [SerializeField] private AudioSource source;
        [SerializeField] private AudioClip lMsgClip;


        private void OnDestroy()
        {
            Path.EndAnimationEvent -= OnWinEvent;
        }
        private void Start()
        {
            Path.EndAnimationEvent+= OnWinEvent;
            //for test
            switch (matrixType)
            {
                case MatrixType.ThreeMultiply:

                    matrix3In3.SetActive(true);
                    break;
                case MatrixType.ForMultiply:

                    matrix4In4.SetActive(true);
                    break;
                case MatrixType.FiveMultilpy:

                    matrix5In5.SetActive(true);
                    break;
            }

            ;
        }

        private void OnWinEvent()
        {
            Animation();
        }
        private void Animation()
        {
            source.PlayOneShot(lMsgClip);
            var tween = lastMsgSprite.DOFade(0, 0.5f);
            tween.onComplete += () => { Invoke(nameof(Win),2f); };
        }

        private void Win()
        {
            InGameController.Instance.Win();
        }
    }

    public enum MatrixType
    {
        ThreeMultiply,
        ForMultiply,
        FiveMultilpy
    }
}