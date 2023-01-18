using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameScene
{
    public class Island : MonoBehaviour
    {
        public static event Action<QuestionData> IslandRefreshQPanelEvent;

        public static string IslandGameRef;
        [SerializeField] private string islandName;
        [SerializeField] private bool isFirst, isOpen, isPassed;
        [SerializeField] private int defStartQuestion;
        [SerializeField] private IslandQuestionButton[] buttons;
        private IslandQuestionButton _clickedButton;
        [SerializeField] private Button gameButton;
        [SerializeField] private GameObject gameLock;
        [SerializeField] private string gameSceneName;
        [SerializeField] private Island nextIsland;
        private Dictionary<int, int> _questionDictionary;
        private int _passedQCount = 0;
        private void OnDestroy()
        {
            QuestionPanel.AnswerEvent -= OnQuestionPanelAnswerEvent;
            QuestionPanel.CancelQuestionEvent -= OnCancelQuestionEvent;
        }
        private void Start()
        {
            QuestionPanel.AnswerEvent += OnQuestionPanelAnswerEvent;
            QuestionPanel.CancelQuestionEvent += OnCancelQuestionEvent;
            foreach (var button in buttons)
            {
                button.OnButtonClickEvent += IslandButtonOnClickEvent;
            }

            gameButton.onClick.AddListener(OnGameButton);
            _questionDictionary = new Dictionary<int, int>();
            var qdJson = PlayerPrefs.GetString(islandName + "Dic", string.Empty);
            if (!string.IsNullOrEmpty(qdJson))
            {
                _questionDictionary = JsonConvert.DeserializeObject<Dictionary<int, int>>(qdJson);
            }

            isOpen = isOpen || PlayerPrefs.GetInt(islandName, 0) > 0;
            Prepare();
        }
        private void OnCancelQuestionEvent()
        {
            _clickedButton = null;
        }
        private void IslandButtonOnClickEvent(IslandQuestionButton clickedButton)
        {
            _clickedButton = clickedButton;
            IslandRefreshQPanelEvent?.Invoke(_clickedButton.questionData);
        }
        private void OnQuestionPanelAnswerEvent(QuestionData questionData, bool con)
        {
            if (con)
            {
                if (_clickedButton is null)
                {
                    return;
                }

                var buttonList = buttons.ToList();

                _clickedButton.HandleCondition();
                var bIndex = buttonList.FindIndex(b => b == _clickedButton);
                if (bIndex >= buttonList.Count - 1)
                {
                    // island is completed and game most be activated.
                    //next island most be activated
                    CompleteIslandQ();
                }
                else
                {
                    //Open next Level
                    buttonList[bIndex + 1].questionData.IsOpen = true;
                    buttonList[bIndex + 1].HandleCondition();
                }

                _clickedButton = null;
            }
            else
            {
                foreach (var b in buttons)
                {
                    if (b.questionData.IsCompleted != questionData.IsCompleted) continue;
                    var data = b.questionData.IsCompleted && isOpen
                        ? GameSceneManager.Instance.GetNextPassed(b.questionData)
                        : GameSceneManager.Instance.GetNextRemain(b.questionData);
                    b.questionData = data ? data : b.questionData;
                }

                if (_clickedButton is not null)
                    IslandRefreshQPanelEvent?.Invoke(_clickedButton.questionData);
            }

            SaveQuestionData();
        }
        private void OnGameButton()
        {
            if (!isPassed) return;
            IslandGameRef = islandName;
            SceneManager.LoadScene(gameSceneName);
        }
        private void Prepare()
        {
            GetQuestionData();
            if (isFirst && !buttons[0].questionData.IsOpen)
            {
                buttons[0].questionData.IsOpen = true;
                buttons[0].HandleCondition();
                PlayerPrefs.SetInt(islandName, 1);
                return;
            }

            foreach (var button in buttons)
            {
                button.HandleCondition();
                if (!button.questionData.IsCompleted)
                {
                    button.questionData.IsOpen = true;
                    break;
                }

                _passedQCount++;
            }

            isPassed = _passedQCount >= buttons.Length;

            gameLock.SetActive(!isPassed);
            if (isPassed && nextIsland != null && !nextIsland.isOpen)
            {
                if (PlayerPrefs.HasKey(nameof(IslandGameRef) + islandName) && nextIsland != null)
                    nextIsland.OpenIsland();
            }

            if (isOpen || isPassed) return;

            foreach (var questionButton in buttons)
            {
                questionButton.Lock();
            }
        }
        private void OpenIsland()
        {
            if (isOpen || PlayerPrefs.GetInt(islandName, 0) > 0)
                return;
            isOpen = true;
            PlayerPrefs.SetInt(islandName, 1);
            if (buttons[0] == null)
                return;
            buttons[0].questionData.IsOpen = true;
            buttons[0].HandleCondition();
            SaveQuestionData();
        }
        private void CompleteIslandQ()
        {
            isPassed = true;
            if (gameLock != null)
                gameLock.SetActive(false);
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