using System;
using Interfaces;
using Painting;
using UnityEngine;

namespace WordTable
{
    public class WordColumn : MonoBehaviour, ITouchable
    {
        private WordTableController _controller;
        [SerializeField] private string value;
        [SerializeField] private SpriteRenderer spRenderer;

        public void SetData(LetterData data)
        {
            spRenderer.sprite = data.sprite;
            value = data.value;
        }

        public void OnBeganTouchHandler()
        {
            if (string.IsNullOrEmpty(_controller.currentWord))
                _controller.AddLetter(value);
        }

        public void OnMoveTouchHandler(Vector3 position)
        {
            if(!string.IsNullOrEmpty(_controller.currentWord))
                _controller.AddLetter(value);
        }

        public void OnEndTouchHandler()
        {
        }

        private void Start()
        {
            _controller = FindObjectOfType<WordTableController>();
        }
    }
}