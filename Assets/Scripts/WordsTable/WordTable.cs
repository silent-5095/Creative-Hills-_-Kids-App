using System;
using Interfaces;
using Painting;
using UnityEngine;

namespace WordsTable
{
    public class WordTable : MonoBehaviour
    {
        private bool _touchIsBegan;
        private WordTableController _tableController;

        public void OnLeaveTable()
        {
            if (_touchIsBegan && _tableController != null)
                _tableController.EndTouch(Vector2.zero);
        }
        private void OnDestroy()
        {
            Detector.BeginTouchEvent -= DetectorOnBeginTouchEvent;
            Detector.EndTouchEvent -= DetectorOnEndTouchEvent;
        }
        private void Start()
        {
            Detector.BeginTouchEvent += DetectorOnBeginTouchEvent;
            Detector.EndTouchEvent += DetectorOnEndTouchEvent;
            _tableController = FindObjectOfType<WordTableController>();
        }
        private void DetectorOnEndTouchEvent(Vector2 pos)
        {
            _touchIsBegan = false;
        }
        private void DetectorOnBeginTouchEvent(Vector2 pos)
        {
            _touchIsBegan = true;
        }
    }
}