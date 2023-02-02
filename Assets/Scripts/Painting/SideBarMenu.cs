using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Interfaces;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Painting
{
    public class SideBarMenu : MonoBehaviour, ITouchable
    {
        // [SerializeField] private Button button;
        [SerializeField] private RectTransform panel;
        [SerializeField] private Button resetButton;
        [SerializeField] private List<MainPaletteButton> mainPaletteButtons;
        private Vector2 _defPos;
        private bool _isActive;

        private void OnDestroy()
        {
            foreach (var paletteButton in mainPaletteButtons)
            {
                paletteButton.OnDestroy();
            }
        }

        private void Start()
        {
            _defPos = panel.anchoredPosition;
            resetButton.onClick.AddListener(ResetAllButtonOnClick);
            // button.onClick.AddListener(AnimateSubPanel);
            foreach (var paletteButton in mainPaletteButtons)
            {
                paletteButton.SetUp();
            }
        }

        private void ResetAllButtonOnClick()
        {
            PaintController.Instance.ResetAll();
            foreach (var paletteButton in mainPaletteButtons)
            {
                paletteButton.OnResetAll();
            }
        }

        public void OnBeganTouchHandler()
        {
        }

        public void OnMoveTouchHandler(Vector3 position)
        {
        }

        public void OnStationaryTouchHandler(Vector3 position)
        {
            
        }

        public void OnMoveTouchHandler()
        {
            // PaintCameraController.Instance.enabled = true;
            // foreach (var paletteButton in mainPaletteButtons)
            // {
            //     paletteButton.OnResetAll();
            // }
            StopCoroutine(ClosePanels());
            StartCoroutine(ClosePanels());
        }

        private IEnumerator ClosePanels()
        {
            PaintCameraController.Instance.enabled = true;
            yield return new WaitForSeconds(0.2f);
            foreach (var paletteButton in mainPaletteButtons)
            {
                paletteButton.OnResetAll();
            }
        }

        public void OnEndTouchHandler()
        {
        }
    }

    [Serializable]
    public class MainPaletteButton
    {
        public string name;
        public static event Action<MainPaletteButton> PushButtonEvent;
        private Guid _id;
        [SerializeField] private Button button;
        [SerializeField] private bool havePalette;
        [SerializeField] private Image onPushedImage;
        [SerializeField] private RectTransform palette;
        [SerializeField] private PaintType paintType;
        [SerializeField] private UnityEvent onclickEvent;
        private Vector2 _paletteDefPos;

        public void SetUp()
        {
            PushButtonEvent += OnCLickButtonEvent;
            button.onClick.AddListener(() =>
            {
                PushButtonEvent?.Invoke(this);
                onclickEvent?.Invoke();
                ShowPalette();
            });
            _id = Guid.NewGuid();
            _paletteDefPos = palette.anchoredPosition;
        }

        private void OnCLickButtonEvent(MainPaletteButton target)
        {
            if (onPushedImage != null)
                onPushedImage.enabled = _id == target._id;
        }

        public void OnDestroy()
        {
            PushButtonEvent -= OnCLickButtonEvent;
        }

        public void ShowPalette()
        {
            var isActive = palette.anchoredPosition.x == 0;
            if (!havePalette)
            {
                var movOutTween = palette.DOAnchorPosX(_paletteDefPos.x, 0.1f);
                movOutTween.onComplete += () =>
                {
                    onclickEvent?.Invoke();
                    movOutTween.Kill();
                };
                return;
            }

            if (isActive)
            {
                var movOutTween = palette.DOAnchorPosX(_paletteDefPos.x, 0.1f);
                movOutTween.onComplete += () =>
                {
                    onclickEvent?.Invoke();
                    var movInTween = palette.DOAnchorPosX(0, 0.1f);
                    movInTween.onComplete += () =>
                    {
                        movOutTween.Kill();
                        movInTween.Kill();
                    };
                };
            }
            else
            {
                onclickEvent?.Invoke();
                var movInTween = palette.DOAnchorPosX(0, 0.1f);
                movInTween.onComplete += () => { movInTween.Kill(); };
            }
        }

        public void OnResetAll()
        {
            onPushedImage.enabled = false;
            if (palette.anchoredPosition == _paletteDefPos)
                return;
            // palette.anchoredPosition = _paletteDefPos;
            var movInTween = palette.DOAnchorPosX(_paletteDefPos.x, 0.1f);
            movInTween.onComplete += () => { movInTween.Kill(); };
        }

        public PaintType PaintType => paintType;
    }
}