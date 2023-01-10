using System;
using UnityEngine;
using UnityEngine.UI;

namespace Painting
{
    public class BucketButton : MonoBehaviour
    {
        public static event Action<Color> BucketClickEvent;
        [SerializeField] private Color color;
        [SerializeField] private Image selectImg;
        [SerializeField] private Button button;

        private void OnDestroy()
        {
            BucketClickEvent -= OnBucketClickEvent;
        }

        private void Start()
        {
            button.image.color = color;
            button.onClick.AddListener(() =>
            {
                // PaintController.SetColor(color);
                BucketClickEvent?.Invoke(color);
            });
            BucketClickEvent += OnBucketClickEvent;
        }

        private void OnBucketClickEvent(Color selectedColor)
        {
            if (selectImg != null)
                selectImg.enabled = color == selectedColor;
        }
    }
}