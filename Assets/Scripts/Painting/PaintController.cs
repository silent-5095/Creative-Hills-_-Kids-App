using System;
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
        
        [SerializeField] private Color defColor;
        [SerializeField] private int defBrush;

        private void OnDestroy()
        {
            BucketButton.BucketClickEvent -= OnBucketClickEvent;
            MainPaletteButton.PushButtonEvent-= OnMainPaletteButtonEvent;
            PatternButton.SelectPatternEvent-= OnPatternButtonClickEvent;
            PatternButton.SelectBrushEvent-= OnSelectBrushButtonClickEvent;
        }

        private void Start()
        {
            _brushIndex = defBrush;
            _currentColor = defColor;
            BucketButton.BucketClickEvent += OnBucketClickEvent;
            MainPaletteButton.PushButtonEvent+= OnMainPaletteButtonEvent;
            PatternButton.SelectPatternEvent+= OnPatternButtonClickEvent;
            PatternButton.SelectBrushEvent+= OnSelectBrushButtonClickEvent;
            Instance = this;
            ResetAll();
        }

        private void OnPatternButtonClickEvent(int index)
        {
            _patternIndex = index;
        }
        private void OnSelectBrushButtonClickEvent(int index)
        {
            _brushIndex = index;
        }

        private void OnMainPaletteButtonEvent(MainPaletteButton target)
        {
            _paintType = target.PaintType;
        }

        private void OnBucketClickEvent(Color currentColor)
        {
            _currentColor = currentColor;
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