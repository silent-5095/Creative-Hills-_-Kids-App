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
        public static event Action<QuestionData> IslandTrueAnswerRefreshEvent;

        public static string IslandGameRef;
        public int islandId;
        [SerializeField] private string islandName;
        [SerializeField] private bool isFirst, isOpen, isPassed;
        [SerializeField] private int defStartQuestion;
        [SerializeField] private IslandQuestionButton[] buttons;
        private IslandQuestionButton _clickedButton;
        [SerializeField] private Button gameButton;
        [SerializeField] private GameObject gameLock;
        [SerializeField] private string gameSceneName;

        [SerializeField] private Island nextIsland;

        // private Dictionary<int, int> _questionDictionary;
        private int _passedQCount = 0;

        private void OnDestroy()
        {
            // QuestionPanel.AnswerEvent -= OnQuestionPanelAnswerEvent;
            QuestionPanel.CancelQuestionEvent -= OnCancelQuestionEvent;
        }

        private void Start()
        {
            // QuestionPanel.AnswerEvent += OnQuestionPanelAnswerEvent;
            QuestionPanel.CancelQuestionEvent += OnCancelQuestionEvent;
            foreach (var button in buttons)
            {
                button.OnButtonClickEvent += IslandButtonOnClickEvent;
            }

            gameButton.onClick.AddListener(OnGameButton);
            // _questionDictionary = new Dictionary<int, int>();
            // var qdJson = PlayerPrefs.GetString(islandName + "Dic", string.Empty);
            // if (!string.IsNullOrEmpty(qdJson))
            // {
            //     _questionDictionary = JsonConvert.DeserializeObject<Dictionary<int, int>>(qdJson);
            // }

            
            // Prepare();
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

        // private void OnQuestionPanelAnswerEvent(QuestionData questionData, bool con)
        // {
        //     Debug.Log(con);
        //     if (con)
        //     {
        //         if (_clickedButton is null)
        //         {
        //             return;
        //         }
        //
        //         var buttonList = buttons.ToList();
        //
        //         _clickedButton.HandleCondition();
        //         var bIndex = buttonList.FindIndex(b => b == _clickedButton);
        //         if (bIndex >= buttonList.Count - 1)
        //         {
        //             CompleteIslandQ();
        //         }
        //         else
        //         {
        //             Debug.Log("openNextLevel");
        //             buttonList[bIndex + 1].questionData.IsOpen = true;
        //             buttonList[bIndex + 1].HandleCondition();
        //         }
        //
        //         _clickedButton = null;
        //     }
        //     else
        //     {
        //         foreach (var b in buttons)
        //         {
        //             if (isOpen)
        //                 b.HandleCondition();
        //             else
        //                 b.Lock();
        //         }
        //
        //         if (_clickedButton is not null)
        //             IslandRefreshQPanelEvent?.Invoke(_clickedButton.questionData);
        //     }
        // }

        private void OnGameButton()
        {
            if (!isPassed) return;
            IslandGameRef = islandName;
            ForDemo.Instance.LoadScene(gameSceneName);
        }

        public void Prepare()
        {
            isOpen = isOpen || PlayerPrefs.GetInt(islandName, 0) > 0;
            foreach (var button in buttons)
            {
                if (isOpen)
                {
                    Debug.Log($"{islandName} is Open");
                    button.HandleCondition();
                }
                else
                    button.Lock();

                if (button.questionData.IsCompleted)
                    _passedQCount++;
            }

            isPassed = _passedQCount >= buttons.Length;

            gameLock.SetActive(!isPassed);
            if (!isPassed || nextIsland == null || nextIsland.isOpen) return;
            if (PlayerPrefs.HasKey(nameof(IslandGameRef) + islandName))
                nextIsland.OpenIsland();
        }

        private void OpenIsland()
        {
            if (isOpen || PlayerPrefs.GetInt(islandName, 0) > 0)
                return;
            isOpen = true;
            PlayerPrefs.SetInt(islandName, 1);
            foreach (var button in buttons)
            {
                button.HandleCondition();
            }
        }

        private void CompleteIslandQ()
        {
            isPassed = true;
            if (gameLock != null)
                gameLock.SetActive(false);
        }
    }
}