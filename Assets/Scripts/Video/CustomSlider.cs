using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Video
{
    public class CustomSlider : Slider
    {
        public event Action<float> OnChangeValueEvent;
        public event Action PointerDownEvent, PointerUpEvent;
        [SerializeField] public bool isTouchable;
        private bool _isDrag;
        protected override void Awake()
        {
            base.Awake();
            if (!isTouchable)
                return;
            onValueChanged.AddListener(call: c =>
            {
                if (_isDrag)
                    OnChangeValueEvent?.Invoke(c);
            });
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            if (!isTouchable) return;
            PointerDownEvent?.Invoke();
            _isDrag = true;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            if (!isTouchable) return;
            _isDrag = false;
            PointerUpEvent?.Invoke();
        }

        public void SetValue(float newValue)
        {
            value = newValue;
        }
    }
}