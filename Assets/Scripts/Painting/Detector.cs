using System;
using Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Painting
{
    public class Detector : MonoBehaviour
    {
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
                    break;
                case TouchPhase.Moved:
                    _currentTouch?.OnMoveTouchHandler(camera.ScreenToWorldPoint(touch.position));
                    break;
                case TouchPhase.Stationary:
                    _currentTouch?.OnMoveTouchHandler(camera.ScreenToWorldPoint(touch.position));
                    break;
                case TouchPhase.Ended:
                    _currentTouch?.OnEndTouchHandler();
                    _currentTouch = null;
                    break;
                case TouchPhase.Canceled:
                    _currentTouch?.OnEndTouchHandler();
                    _currentTouch = null;
                    break;
                default:
                    if (_currentTouch is null)
                        break;
                    _currentTouch?.OnEndTouchHandler();
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