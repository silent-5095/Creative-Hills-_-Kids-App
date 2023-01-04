using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WordTable
{
    public class WordTableController : MonoBehaviour
    {
        public List<string> currentWord;
        private bool _isStarted;

        private void Awake()
        {
        }

        public void AddLetter(string a)
        {
            if (!_isStarted)
                _isStarted = true;
            currentWord.Add(a);
        }

        public void EndTouch()
        {
            Debug.Log("End Touch ");
            Debug.Log($"word is {currentWord}");
        }
    }
}