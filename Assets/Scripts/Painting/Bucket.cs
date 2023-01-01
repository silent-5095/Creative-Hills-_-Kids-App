using System;
using UnityEngine;
using UnityEngine.UI;

namespace Painting
{
    public class Bucket : MonoBehaviour
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
            button.onClick.AddListener(()=>BucketClickEvent?.Invoke(color));
            BucketClickEvent+= OnBucketClickEvent;
        }

        private void OnBucketClickEvent(Color selectedColor)
        {
            selectImg.enabled = color == selectedColor;
        }
    }
}
