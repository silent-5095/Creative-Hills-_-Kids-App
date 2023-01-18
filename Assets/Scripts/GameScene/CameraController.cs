using System;
using Painting;
using UnityEngine;

namespace GameScene
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private float speed = 0.5f, touchOffset;
        [SerializeField] private Transform minBorder, maxBorder;
        private Vector2 _startPos;
        private bool _touchStarted;
        private float _islandCamLastPos = 0;

        private void OnDestroy()
        {
            Detector.BeginTouchEvent -= DetectorOnBeginTouchEvent;
            Detector.MoveTouchEvent -= OnMoveTouchEvent;
            Detector.EndTouchEvent -= OnEndTouch;
            PlayerPrefs.SetFloat(nameof(_islandCamLastPos),_islandCamLastPos);
        }

        private void Start()
        {
            Detector.BeginTouchEvent += DetectorOnBeginTouchEvent;
            Detector.MoveTouchEvent += OnMoveTouchEvent;
            Detector.EndTouchEvent += OnEndTouch;
            
            var tempPos=mainCamera.transform.position;
                tempPos.x=PlayerPrefs.GetFloat(nameof(_islandCamLastPos),_islandCamLastPos);
                mainCamera.transform.position = tempPos;
        }


        private void DetectorOnBeginTouchEvent(Vector2 pos)
        {
            _startPos = pos;
            _touchStarted = true;
        }

        private void OnMoveTouchEvent(Vector2 pos)
        {
            if (!_touchStarted)
                return;
            var tempPos = mainCamera.transform.position;
            if (Mathf.Abs(pos.x - _startPos.x) <= touchOffset)
            {
                return;
            }

            tempPos.x += Mathf.Sign(_startPos.x -pos.x) * speed * Time.fixedDeltaTime;
            if (tempPos.x > minBorder.position.x && tempPos.x < maxBorder.position.x)
            {
                mainCamera.transform.position = tempPos;
                _islandCamLastPos = tempPos.x;
            }
        }
        private void OnEndTouch(Vector2 pos)
        {
            _touchStarted = false;
        }
    }
}