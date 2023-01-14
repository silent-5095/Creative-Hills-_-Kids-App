using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace GameScene
{
    public class Island : MonoBehaviour
    {
        [SerializeField] private string islandName;
        [SerializeField] private bool isFirst, isOpen, isPassed;
        [SerializeField] private int defStartQuestion;
        [SerializeField] private IslandQuestionButton[] buttons;
        [SerializeField] private Island nextIsland;
        private Dictionary<int, int> _questionDictionary;
        private int _passedQCount = 0;

        private void OnDestroy()
        {
            QuestionPanel.AnswerEvent -= OnQuestionPanelAnswerEvent;
        }

        private void Start()
        {
            QuestionPanel.AnswerEvent += OnQuestionPanelAnswerEvent;
            _questionDictionary = new Dictionary<int, int>();
            var qdJson = PlayerPrefs.GetString(islandName + "Dic", string.Empty);
            if (!string.IsNullOrEmpty(qdJson))
            {
                _questionDictionary = JsonConvert.DeserializeObject<Dictionary<int, int>>(qdJson);
            }

            isOpen = isOpen || PlayerPrefs.GetInt(islandName, 0) > 0;
            _passedQCount = _passedQCount > defStartQuestion ? _passedQCount : defStartQuestion;
            isPassed = _passedQCount >= buttons.Length;
            Prepare();
        }
        private void Prepare()
        {
            GetQuestionData();
            if (isFirst && !buttons[0].questionData.IsOpen)
            {
                buttons[0].questionData.IsOpen = true;
            }

            if (isOpen || isPassed)
            {
                foreach (var button in buttons)
                {
                    button.HandleCondition();
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
            Debug.Log($"{name} dic is ={_questionDictionary is null}");

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
                    if (i + defStartQuestion >= GameSceneManager.Instance.TotalQuestion())
                        buttons[i].questionData = GameSceneManager.Instance.GetQuestionById(_questionDictionary[i]);
                }
        }

        private void OnQuestionPanelAnswerEvent(QuestionData questionData)
        {
            var buttonList = buttons.ToList();
            var button = buttons.FirstOrDefault(b => b.questionData == questionData);
            if (button is null)
                return;
            if (questionData.IsCompleted)
            {
                button.HandleCondition();
                var bIndex = buttonList.FindIndex(b => b == button);
                if (bIndex >= buttonList.Count-1)
                {
                    // island is completed and game most be activated.
                    //next island most be activated
                    CompleteIsland();
                }
                else
                {
                    //Open next Level
                    Debug.Log($"{name} bIndex= {bIndex}");
                    Debug.Log($"{name} buttonList.Count= {buttonList.Count}");
                    buttonList[bIndex + 1].questionData.IsOpen = true;
                    buttonList[bIndex + 1].HandleCondition();
                }
            }
            else
            {
                button.questionData = GameSceneManager.Instance.GetNextQuestion(button.questionData);
            }

            SaveQuestionData();
        }


        public void OpenIsland()
        {
            if (isOpen)
                return;
            isOpen = true;
            buttons[0].questionData.IsOpen = true;
            buttons[0].HandleCondition();
            SaveQuestionData();
        }

        private void CompleteIsland()
        {
            isPassed = true;
            if (nextIsland == null)
                return;
            nextIsland.OpenIsland();
        }


        private void SaveQuestionData()
        {
            _questionDictionary = new Dictionary<int, int>();
            for (var i = 0; i < buttons.Length; i++)
            {
                _questionDictionary.Add(i, buttons[i].questionData.QuestionId);
            }
        }
    }
}