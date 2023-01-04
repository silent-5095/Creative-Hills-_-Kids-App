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

        public void OnBeganTouchHandler()
        {
            if (_controller.currentWord.Count==0)
                _controller.AddLetter(value);
        }

        public void OnMoveTouchHandler(Vector3 position)
        {
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