using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace GameScene
{
    public class Island : MonoBehaviour
    {
        [SerializeField] private string islandName;
        [SerializeField] private bool isOpen, isPassed;
        [SerializeField] private int defStartQuestion;
        [SerializeField] private QuestionPanel qPanel;
        [SerializeField] private IslandQuestionButton[] buttons;
        private Dictionary<int, int> _questionDictionary;
        private int _questionIndex = 0;

        private void Start()
        {
            _questionDictionary = new Dictionary<int, int>();
            var qdJson = PlayerPrefs.GetString(islandName + "Dic", string.Empty);
            if (!string.IsNullOrEmpty(qdJson))
            {
                _questionDictionary = JsonConvert.DeserializeObject<Dictionary<int, int>>(qdJson);
            }

            isOpen = isOpen || PlayerPrefs.GetInt(islandName, 0) > 0;
            _questionIndex = _questionIndex > defStartQuestion ? _questionIndex : defStartQuestion;
            isPassed = _questionIndex >= buttons.Length;
            Prepare();
            GetQuestionData();
        }

        private void Prepare()
        {
            if (isOpen)
            {
                // in end
                if (isPassed)
                    foreach (var questionButton in buttons)
                    {
                        questionButton.Pass();
                    }
                else
                {
                    for (var i = 0; i < _questionIndex; i++)
                    {
                        if (buttons.Length <= i)
                            break;
                        buttons[i].Pass();
                    }

                    buttons[_questionIndex].Open();
                }
            }
            else
            {
                foreach (var questionButton in buttons)
                {
                    questionButton.Lock();
                }
            }
        }

        private void GetQuestionData()
        {
            if (_questionDictionary is null || _questionDictionary.Count <= 0)
            {
                for (var i = 0; i < buttons.Length; i++)
                {
                    buttons[i].questionData = GameSceneManager.Instance.GetQuestion(i + defStartQuestion);
                }
            }
            else
                for (var i = 0; i < buttons.Length; i++)
                {
                    buttons[i].questionData = GameSceneManager.Instance.GetQuestionById(_questionDictionary[i]);
                }
        }

        private void SaveQuestionData()
        {
            _questionDictionary=new Dictionary<int, int>();
            for (var i = 0; i < buttons.Length; i++)
            {
                _questionDictionary.Add(i, buttons[i].questionData.QuestionId);
            }
        }
    }
}