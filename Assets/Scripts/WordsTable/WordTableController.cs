using System;
using System.Collections.Generic;
using Painting;
using RTLTMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WordsTable
{
    public class WordTableController : MonoBehaviour
    {
        public static event Action<string> CompleteWordEvent;
        [SerializeField] private AudioSource source;
        [SerializeField] private WinPanel winPanel;
         private int _wordCount;
        [SerializeField] private LineController wordLinePrefab;
        private LineController _currentLineRenderer;
        public string currentWord;
        [SerializeField] private RTLTextMeshPro currentWordTxt;
        [SerializeField] private List<string> levelWord;
        private List<LineController> _currentLineControllers;
        [SerializeField] private List<Color> lineColors;
        private Color _currentColor;

        private void OnDestroy()
        {
            Detector.EndTouchEvent -= EndTouch;
            Detector.MoveTouchEvent -= MoveTouch;
            CompleteWordEvent = null;
        }

        private void Awake()
        {
            _wordCount = levelWord.Count;
            Detector.EndTouchEvent += EndTouch;
            Detector.MoveTouchEvent += MoveTouch;
            CompleteWordEvent += s =>
            {
                _wordCount -= 1;
                if (_wordCount <= 0)
                {
                    winPanel.ShowWinPanel();
                }
                else
                    source.Play();
            };
        }

        private void Start()
        {
            currentWord = string.Empty;
            _currentLineControllers = new List<LineController>();
            _currentColor = lineColors[Random.Range(0, lineColors.Count)];
            lineColors.Remove(_currentColor);
        }

        private void Update()
        {
            currentWordTxt.text = currentWord;
        }

        // ReSharper disable once Unity.NoNullPropagation
        public void AddLetter(WordColumn column)
        {
            var position = column.transform.position;

            var tempLine = Instantiate(wordLinePrefab);
            // tempLine.AddStartingNode(_currentLineRenderer == null ? position : _currentLineRenderer.GetLastPos());
            tempLine.AddStartingNode(position);
            if (_currentLineRenderer != null)
                _currentLineRenderer.MoveLastPoint(position);
            _currentLineRenderer = tempLine;
            _currentLineRenderer.SetColor(_currentColor);
            Debug.Log(_currentLineRenderer.transform.position);

            _currentLineControllers ??= new List<LineController>();
            _currentLineControllers.Add(_currentLineRenderer);
            currentWord += column.Value;
        }

        private void MoveTouch(Vector2 position)
        {
            if (_currentLineRenderer != null)
                _currentLineRenderer.MoveLastPoint(new Vector3(position.x, position.y, 1));
        }

        public void EndTouch(Vector2 pos)
        {
            var contains = false;
            foreach (var word in levelWord)
            {
                contains = currentWord.Equals(word);
                if (!contains) continue;
                if (_currentLineRenderer is not null)
                    Destroy(_currentLineRenderer.gameObject);
                break;
            }

            if (contains)
            {
                CompleteWordEvent?.Invoke(currentWord);
                currentWord = string.Empty;
                _currentLineControllers = null;
                if (lineColors is null || lineColors.Count <= 0)
                    return;
                _currentColor = lineColors[Random.Range(0, lineColors.Count)];
                lineColors.Remove(_currentColor);
            }
            else
            {
                currentWord = string.Empty;
                if (_currentLineControllers is not null)
                    foreach (var line in _currentLineControllers)
                    {
                        Debug.Log(_currentLineControllers.Count);
                        Destroy(line.gameObject);
                    }

                _currentLineControllers = null;
            }
        }
    }
}