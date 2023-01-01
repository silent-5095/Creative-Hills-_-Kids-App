using System;
using UnityEngine;

namespace Painting
{
    public class PaintController : MonoBehaviour
    {
        private static Color _currentColor;
        [SerializeField] private Color testColor;

        private void OnDestroy()
        {
            Bucket.BucketClickEvent -= OnBucketClickEvent;
        }

        private void Start()
        {
            _currentColor = testColor;
            Bucket.BucketClickEvent+= OnBucketClickEvent;
        }

        private void OnBucketClickEvent(Color currentColor)
        {
            _currentColor = currentColor;
        }

        public static Color GetColor()
        {
            return _currentColor;
        }
    }
}
