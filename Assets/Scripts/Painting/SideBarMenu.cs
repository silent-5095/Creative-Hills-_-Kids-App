using DG.Tweening;
using Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace Painting
{
    public class SideBarMenu : MonoBehaviour,ITouchable
    {
        [SerializeField] private Button button;
        [SerializeField] private RectTransform panel;
        private Vector2 _defPos;
        private bool _isActive;

        private void Start()
        {
            _defPos = panel.anchoredPosition;
            button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            // panel.anchoredPosition = _isActive ? _defPos : Vector2.zero;
            var tween=panel.DOAnchorPosX(_isActive ? _defPos.x : 0, 0.5f);
            tween.onComplete += () => tween.Kill();
            _isActive = !_isActive;
        }

        public void OnBeganTouchHandler()
        {
            
        }

        public void OnMoveTouchHandler(Vector3 position)
        {
            
        }

        public void OnMoveTouchHandler()
        {
            
        }

        public void OnEndTouchHandler()
        {
            
        }
    }
}