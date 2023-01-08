using System;
using Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Painting
{
    public class Detector : MonoBehaviour
    {
        public static event Action BeginTouchEvent;
        public static event Action<Vector2> MoveTouchEvent;
        public static event Action EndTouchEvent;
        [SerializeField] private new Camera camera;
        [SerializeField] private LayerMask layer;
        private bool _isEnded;
        private ITouchable _currentTouch;

        private void Update()
        {
            if (_isEnded)
                return;
            if (Input.touchCount <= 0) return;
            var touch = Input.touches[0];
            Vector2 pos = camera.ScreenToWorldPoint(touch.position);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // if (touch.phase != TouchPhase.Began) return;
                    var x = Physics2D.Raycast(camera.ScreenToWorldPoint(touch.position), Vector2.zero, layer);
                    if ((x.collider != null ? x.collider.gameObject : null) is not null)
                    {
                        if (x.collider.gameObject.TryGetComponent(typeof(ITouchable), out var com))
                        {
                            _currentTouch = (ITouchable) com;
                            _currentTouch.OnBeganTouchHandler();
                        }
                    }
                    BeginTouchEvent?.Invoke();
                    break;
                case TouchPhase.Moved:
                    _currentTouch?.OnMoveTouchHandler(pos);
                    MoveTouchEvent?.Invoke(pos);
                    break;
                case TouchPhase.Stationary:
                    _currentTouch?.OnMoveTouchHandler(pos);
                    MoveTouchEvent?.Invoke(pos);
                    break;
                case TouchPhase.Ended:
                    _currentTouch?.OnEndTouchHandler();
                    EndTouchEvent?.Invoke();
                    _currentTouch = null;
                    break;
                case TouchPhase.Canceled:
                    _currentTouch?.OnEndTouchHandler();
                    EndTouchEvent?.Invoke();
                    _currentTouch = null;
                    break;
                default:
                    if (_currentTouch is null)
                        break;
                    _currentTouch?.OnEndTouchHandler();
                    EndTouchEvent?.Invoke();
                    _currentTouch = null;
                    break;
            }
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}