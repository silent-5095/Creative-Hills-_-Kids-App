using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace GameScene
{
    public class Island : MonoBehaviour
    {
        public static event Action<bool> IslandRefreshEvent;
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
            IslandRefreshEvent -= OnRefreshEvent;
        }

        private void Start()
        {
            QuestionPanel.AnswerEvent += OnQuestionPanelAnswerEvent;
            IslandRefreshEvent += OnRefreshEvent;
            _questionDictionary = new Dictionary<int, int>();
            var qdJson = PlayerPrefs.GetString(islandName + "Dic", string.Empty);
            if (!string.IsNullOrEmpty(qdJson))
            {
                _questionDictionary = JsonConvert.DeserializeObject<Dictionary<int, int>>(qdJson);
            }

            isOpen = isOpen || PlayerPrefs.GetInt(islandName, 0) > 0;
            Prepare();
        }

        private void OnRefreshEvent(bool con)
        {
            if (isPassed)
                return;
            foreach (var b in buttons)
            {
                if (b.questionData.IsCompleted != con)
                    continue;
                b.questionData = b.questionData.IsCompleted
                    ? GameSceneManager.Instance.GetNextPassed(b.questionData)
                    : GameSceneManager.Instance.GetNextRemain(b.questionData);


            }

            SaveQuestionData();
        }

        private void Prepare()
        {
            GetQuestionData();
            if (isFirst && !buttons[0].questionData.IsOpen)
            {
                buttons[0].questionData.IsOpen = true;
                buttons[0].HandleCondition();
                PlayerPrefs.SetInt(islandName, 1);
            }
            foreach (var button in buttons)
            {
                if (button.questionData.IsCompleted)
                    _passedQCount++;
            }

            isPassed = _passedQCount >= buttons.Length;

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
            if (_questionDictionary is null || _questionDictionary.Count <= 0)
            {
                for (var i = 0; i < buttons.Length; i++)
                {
                    buttons[i].questionData = GameSceneManager.Instance.GetQuestion(i + defStartQuestion);
                }

                SaveQuestionData();
            }
            else
            {
                for (var i = 0; i < buttons.Length; i++)
                {
                    if (i + defStartQuestion <= GameSceneManager.Instance.TotalQuestion())
                        buttons[i].questionData = GameSceneManager.Instance.GetQuestionById(_questionDictionary[i]);
                }
            }
        }

        private void OnQuestionPanelAnswerEvent(QuestionData questionData, bool con)
        {
            var buttonList = buttons.ToList();
            var button = buttons.FirstOrDefault(b => b.questionData == questionData);
            if (button is null)
                return;
            if (con)
            {
                button.HandleCondition();
                var bIndex = buttonList.FindIndex(b => b == button);
                if (bIndex >= buttonList.Count - 1)
                {
                    // island is completed and game most be activated.
                    //next island most be activated
                    CompleteIsland();
                }
                else
                {
                    //Open next Level
                    buttonList[bIndex + 1].questionData.IsOpen = true;
                    buttonList[bIndex + 1].HandleCondition();
                }

                SaveQuestionData();
            }
            else
            {
                IslandRefreshEvent?.Invoke(questionData.IsCompleted);
            }
        }


        public void OpenIsland()
        {
            if (isOpen)
                return;
            isOpen = true;
            PlayerPrefs.SetInt(islandName, 1);
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

            var qdJson = JsonConvert.SerializeObject(_questionDictionary);
            PlayerPrefs.SetString(islandName + "Dic", qdJson);
        }
    }
}