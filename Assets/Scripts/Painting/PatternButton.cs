using System;
using UnityEngine;
using UnityEngine.UI;

namespace Painting
{
    [RequireComponent(typeof(Button))]
    public class PatternButton : MonoBehaviour
    {
        public static event Action<int> SelectPatternEvent;
        public static event Action<int> SelectBrushEvent;
        [SerializeField] private int id;
        [SerializeField] private bool isBrushButton;
        [SerializeField] private Image selectImg;
        [SerializeField] private Button button;

        private void Start()
        {
            button.onClick.AddListener(OnButtonClick);
            if (!isBrushButton)
                SelectPatternEvent += OnPushedButtonEvent;
            else
                SelectBrushEvent += OnPushedButtonEvent;
        }

        private void OnButtonClick()
        {
            if (!isBrushButton)
                SelectPatternEvent?.Invoke(id);
            else
                SelectBrushEvent ?.Invoke(id);
        }

        private void OnPushedButtonEvent(int targetId)
        {
            if (selectImg != null)
                selectImg.enabled = id == targetId;
        }
    }
}