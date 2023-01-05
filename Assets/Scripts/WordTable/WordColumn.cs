using System;
using Interfaces;
using Painting;
using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WordTable
{
    public class WordColumn : MonoBehaviour/* , ITouchable */
    {
        private WordTableController _controller;
        [SerializeField] private string value;
        [SerializeField] private Image spRenderer;
        [SerializeField] private RTLTextMeshPro text;
        [SerializeField] private bool update;

        public string Value => value;
        private bool _isAdded;

        public void SetData(WordProp data)
        {
            spRenderer.sprite = data.sprite;
            value = data.value;
            text.text = value;
        }

        public void OnBeganTouchHandler()
        {
            Debug.Log($"On began in {value}");
            if (string.IsNullOrEmpty(_controller.currentWord))
            {
                Debug.Log($"On began in {value}");
                _controller.AddLetter(value);
                _isAdded = true;
            }
        }

        public void OnMoveTouchHandler(Vector3 position)
        {
            if(!string.IsNullOrEmpty(_controller.currentWord) && !_isAdded)
            {
                Debug.Log($"On Move in {value}");
                _controller.AddLetter(value);
                _isAdded = true;
            }
        }
        public void OnMoveTouchHandler()
        {
            if(!string.IsNullOrEmpty(_controller.currentWord) && !_isAdded)
            {
                Debug.Log($"On Move in {value}");
                _controller.AddLetter(value);
                _isAdded = true;
            }
        }

        public void OnEndTouchHandler()
        {
            _isAdded = false;
        }

       private void OnValidate()
       {
           if (update)
           {
               update = false;
               SetData(new WordProp
               {
                   sprite = null,
                   value=value
               });
           }
       }

        private void OnDestroy()
        {
            Detector.EndTouchEvent -= OnEndTouchHandler;
        }

        private void Start()
        {
            Detector.EndTouchEvent += OnEndTouchHandler;
            _controller = FindObjectOfType<WordTableController>();
            SetData(new WordProp
            {
                sprite = spRenderer.sprite,
                value=value
            });
            _isAdded = false;
        }
    }
}