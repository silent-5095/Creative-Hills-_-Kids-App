using System;
using System.Collections.Generic;
using System.Linq;
using Painting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WordTable
{
    public class WordTableController : MonoBehaviour
    {
        public static event Action<string> CompleteWordEvent;
        public string currentWord;
        [SerializeField] private List<string> levelWord;
        [SerializeField] private WordData data;
        private bool _isStarted;
        [SerializeField] private List<WordColumn> columns;

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
            while (columns is not null && columns.Count > 0)
            {
                var index = Random.Range(0, columns.Count);
                //WordData
                //TODO randomize of letters 
            }
        }

        public void AddLetter(string a)
        {
            if (!_isStarted)
                _isStarted = true;
            currentWord += a;
        }

        private void EndTouch()
        {
            Debug.Log("End Touch ");
            Debug.Log($"word is {currentWord}");

            var contains = levelWord.Contains(currentWord);
            if (contains)
            {
                Debug.Log("have Word");
                CompleteWordEvent?.Invoke(currentWord);
            }
            else
            {
                Debug.Log("No Word");
                currentWord = string.Empty;
            }
        }

        private void LineCreator()
        {
        }
    }
}