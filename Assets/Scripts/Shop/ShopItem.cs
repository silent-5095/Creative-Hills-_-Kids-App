using System;
using System.Collections;
using Painting;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class ShopItem : MonoBehaviour
    {
        public event Action AttachEvent;
        public event Action<ShopItem> EndDragEvent;
        [SerializeField] private bool isActive;
        [SerializeField] private ShopItemList tickImage;
        [SerializeField] private GameObject inBasketObj;

        public bool IsActive => isActive;

        private Vector2 _defPos;
        private bool _isTouched, _isAttached;

        private void OnDestroy()
        {
            Detector.MoveTouchEvent -= OnMoveEvent;
        }

        private void Start()
        {
            Detector.MoveTouchEvent += OnMoveEvent;
            _defPos = transform.position;
        }

        private void OnMoveEvent(Vector2 pos)
        {
            if (_isTouched && !_isAttached)
            {
                transform.position = pos;
            }
        }

        public void OnPointerDown()
        {
            if (!_isAttached)
                _isTouched = true;
            
        }

        public void OnPointerUp()
        {
            if (!_isAttached)
                EndDragEvent?.Invoke(this);
            _isTouched = false;
        }

        public void Reset()
        {
            StartCoroutine(ResetPosition());
        }

        private IEnumerator ResetPosition()
        {
            yield return new WaitForSeconds(0.01f);
            if (!_isAttached)
            {
                transform.position = _defPos;
            }
        }

        public void AttachToSlot()
        {
            _isAttached = true;
            _isTouched = false;
            tickImage.ActiveTick();
            inBasketObj.SetActive(true);
            AttachEvent?.Invoke();
            gameObject.SetActive(false);
        }
    }
}