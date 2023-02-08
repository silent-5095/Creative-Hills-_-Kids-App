using System;
using System.Collections.Generic;
using UnityEngine;

namespace Painting
{
    public class PaintController : MonoBehaviour
    {
        public static event Action ResetAllEvent;
        public static PaintController Instance;
        private static Color _currentColor;
        private static int _patternIndex;
        private static int _brushIndex;
        private static PaintType _paintType;
        [SerializeField] private AudioSource source;
        
        [SerializeField] private Color defColor;
        [SerializeField] private int defBrush;
        public float touchDly;

        private void OnDestroy()
        {
            BucketButton.BucketClickEvent -= OnBucketClickEvent;
            MainPaletteButton.PushButtonEvent-= OnMainPaletteButtonEvent;
            PatternButton.SelectPatternEvent-= OnPatternButtonClickEvent;
            PatternButton.SelectBrushEvent-= OnSelectBrushButtonClickEvent;
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _brushIndex = defBrush;
            _currentColor = defColor;
            BucketButton.BucketClickEvent += OnBucketClickEvent;
            MainPaletteButton.PushButtonEvent+= OnMainPaletteButtonEvent;
            PatternButton.SelectPatternEvent+= OnPatternButtonClickEvent;
            PatternButton.SelectBrushEvent+= OnSelectBrushButtonClickEvent;
            // ResetAll();
        }

        private void OnPatternButtonClickEvent(int index)
        {
            _patternIndex = index;
            source.Play();
        }
        private void OnSelectBrushButtonClickEvent(int index)
        {
            _brushIndex = index;
            source.Play();
        }

        private void OnMainPaletteButtonEvent(MainPaletteButton target)
        {
            _paintType = target.PaintType;
            source.Play();
        }

        private void OnBucketClickEvent(Color currentColor)
        {
            _currentColor = currentColor;
            source.Play();
        }

        public static Color GetColor()
        {
            return _currentColor;
        }

        public static PaintType GetPaintType()
        {
            return _paintType;
        }

        public static int GetPatternIndex() => _patternIndex;
        public static int GetBrushIndex() => _brushIndex;

        public void ResetAll()
        {
            _currentColor = defColor;
            _brushIndex = defBrush;
            _paintType = PaintType.Color;
            ResetAllEvent?.Invoke();
        }
    }

    public enum PaintType
    {
        None,
        Color,
        Texture,
        Brush,
        Erase
    }
}