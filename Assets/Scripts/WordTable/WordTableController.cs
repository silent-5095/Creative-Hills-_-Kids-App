using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Painting;
using RTLTMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WordTable
{
    public class WordTableController : MonoBehaviour
    {
        public static event Action<string> CompleteWordEvent;
        public string currentWord;
        [SerializeField] private RTLTextMeshPro currentWordTxt;
        [SerializeField] private List<string> levelWord;
        // private bool _isStarted;

        private void OnDestroy()
        {
            Detector.EndTouchEvent -= EndTouch;
        }

        private void Awake()
        {
            Detector.EndTouchEvent += EndTouch;
        }

        private void Start()
        {
            currentWord = string.Empty;
            // _charList = new List<char>();
            // foreach (var word in levelWord)
            // {
            //     foreach (var wchar in word.ToCharArray())
            //     {
            //         if (_charList.Contains(wchar))
            //             continue;
            //         _charList.Add(wchar);
            //     }
            // }
        }

        private void Update()
        {
            currentWordTxt.text = currentWord;
        }

        public void AddLetter(string a)
        {
            // if (!_isStarted)
            //     _isStarted = true;
            currentWord += a;
            Debug.Log($"AddLetter= {a} --- and current is ={currentWord}");
        }

        private void EndTouch()
        {
            Debug.Log("End Touch ");
            Debug.Log($"word is {currentWord}");

            var contains = false;
            foreach (var word in levelWord)
            {
                contains = currentWord.Equals(word);
                if (contains)
                    break;
            }

            if (contains)
            {
                Debug.Log("have Word");
                CompleteWordEvent?.Invoke(currentWord);
                currentWord = string.Empty;
            }
            else
            {
                Debug.Log("No Word");
                currentWord = string.Empty;
            }
        }
    }
}