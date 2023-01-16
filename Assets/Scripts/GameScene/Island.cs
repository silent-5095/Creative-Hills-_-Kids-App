using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Scene = UnityEngine.SceneManagement.Scene;

namespace GameScene
{
    public class Island : MonoBehaviour
    {
        public static event Action<QuestionData> IslandRefreshQPanelEvent;
        public static event Action<Island, QuestionData> IslandRefreshButtonsEvent;
        public static string IslandGameRef;
        [SerializeField] private string islandName;
        [SerializeField] private bool isFirst, isOpen, isPassed;
        [SerializeField] private int defStartQuestion;
        [SerializeField] private IslandQuestionButton[] buttons;
        [SerializeField] private Button gameButton;
        [SerializeField] private GameObject gameLock;
        [SerializeField] private string gameSceneName;
        [SerializeField] private Island nextIsland;
        private Dictionary<int, int> _questionDictionary;
        public IslandQuestionButton[] GetButtons => buttons;
        private int _passedQCount = 0;

        private void OnDestroy()
        {
            QuestionPanel.AnswerEvent -= OnQuestionPanelAnswerEvent;
            IslandRefreshButtonsEvent -= OnRefreshEvent;
        }

        private void Start()
        {
            QuestionPanel.AnswerEvent += OnQuestionPanelAnswerEvent;
            IslandRefreshButtonsEvent += OnRefreshEvent;
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

        private void OnGameButton()
        {
            if (isPassed)
            {
                IslandGameRef = islandName;
                Debug.Log(IslandGameRef + "island name is =" + islandName);
                SceneManager.LoadScene(gameSceneName);
            }
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
                    CompleteIslandQ();
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
                OnWrongAnswer(questionData);
            }
        }

        private void OnWrongAnswer(QuestionData questionData)
        {
            // if (isPassed)
            //     return;
            var targetButton = buttons.FirstOrDefault(b => b.questionData == questionData);
            Debug.Log($"targetButton data ={targetButton?.questionData.name}");
            foreach (var b in buttons)
            {
                if (b.questionData.IsCompleted != questionData.IsCompleted) continue;
                var data = b.questionData.IsCompleted && isOpen
                    ? GameSceneManager.Instance.GetNextPassed(b.questionData)
                    : GameSceneManager.Instance.GetNextRemain(b.questionData);
                b.questionData = data ? data : b.questionData;
                // Debug.Log($"data =>{b.questionData.name} and complete ={b.questionData.IsCompleted}",
                    // b.gameObject);
            }
            Debug.Log($"targetButton data after change ={targetButton?.questionData.name}");

            if (targetButton is not null)
                IslandRefreshQPanelEvent?.Invoke(targetButton.questionData);
            SaveQuestionData();
        }

        private void OnRefreshEvent(Island island, QuestionData questionData)
        {
            // if (isPassed)
            //     return;
            if (island == this)
                return;
            foreach (var b in buttons)
            {
                if (b.questionData.IsCompleted != questionData.IsCompleted) continue;
                var data = b.questionData.IsCompleted && isOpen
                    ? GameSceneManager.Instance.GetNextPassed(b.questionData)
                    : GameSceneManager.Instance.GetNextRemain(b.questionData);
                b.questionData = data ? data : b.questionData;
                Debug.Log($"data =>{b.questionData.name} and complete ={b.questionData.IsCompleted}",
                    b.gameObject);
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