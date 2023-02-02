using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene
{
    public class Island : MonoBehaviour
    {
        public static event Action<QuestionData> IslandRefreshQPanelEvent;
        public static event Action<Transform> CompleteIslandEvent;
        public static string IslandGameRef;
        [SerializeField] private string islandName;
        [SerializeField] private bool isFirst, isOpen, isPassed, isGamePassed;
        [SerializeField] private IslandQuestionButton[] buttons;
        private IslandQuestionButton _clickedButton;
        [SerializeField] public Button gameButton;
        [SerializeField] private GameObject gameLock;
        [SerializeField] private string gameSceneName;
        [SerializeField] private Island nextIsland;
        private int _passedQCount = 0;

        private void OnDestroy()
        {
            QuestionPanel.CancelQuestionEvent -= OnCancelQuestionEvent;
        }

        private void Start()
        {
            QuestionPanel.CancelQuestionEvent += OnCancelQuestionEvent;
            foreach (var button in buttons)
            {
                button.OnButtonClickEvent += IslandButtonOnClickEvent;
            }

            gameButton.onClick.AddListener(OnGameButton);

            isGamePassed = PlayerPrefs.GetInt(islandName + "Game") > 0;
            if (IslandGameRef == islandName && !isGamePassed)
            {
                CompleteIslandEvent?.Invoke(nextIsland.transform);
                PlayerPrefs.SetInt(islandName + "Game", 1);
                isGamePassed = true;
                IslandGameRef = string.Empty;
                nextIsland?.OpenIsland();
            }
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

        private void OnGameButton()
        {
            if (!isPassed) return;
            IslandGameRef = islandName;
            ForDemo.Instance.LoadScene(gameSceneName);
        }
        // public void Prepare()
        // {
        //     isOpen = isOpen || PlayerPrefs.GetInt(islandName, 0) > 0;
        //     foreach (var button in buttons)
        //     {
        //         if (isOpen)
        //         {
        //             button.HandleCondition();
        //         }
        //         else
        //             button.Lock();
        //
        //         if (button.questionData.IsCompleted)
        //             _passedQCount++;
        //     }
        //     isPassed = _passedQCount >= buttons.Length;
        //     gameLock.SetActive(!isPassed);
        //     if (!isPassed || nextIsland == null || nextIsland.isOpen) return;
        //     if (PlayerPrefs.HasKey(nameof(IslandGameRef) + islandName))
        //         nextIsland.OpenIsland();
        // }

        public void OnQuestionAnswer()
        {
            isOpen = isFirst || PlayerPrefs.GetInt(islandName + "Open") > 0;
            isPassed = PlayerPrefs.GetInt(islandName + "Passed") > 0;
            gameLock.SetActive(!(PlayerPrefs.GetInt(islandName + "GameLock") > 0));
            if (isPassed)
            {
                foreach (var button in buttons)
                {
                    Debug.Log("Answer In passed");

                    button.Pass();
                }

                return;
            }

            if (!isOpen) return;
            var passedQ = 0;
            foreach (var button in buttons)
            {
                if (button.questionData.IsCompleted)
                    passedQ++;
                button.HandleCondition();
                Debug.Log("Answer In not passed");
            }

            isPassed = passedQ >= buttons.Length;
            PlayerPrefs.SetInt(islandName + "Passed", isPassed ? 1 : 0);
            PlayerPrefs.SetInt(islandName + "GameLock", isPassed ? 1 : 0);
            if (isPassed)
            {
                CompleteIslandEvent?.Invoke(gameButton.transform);
                gameLock.SetActive(false);
            }
        }

        private void OpenIsland()
        {
            isOpen = true;
            foreach (var button in buttons)
            {
                button.HandleCondition();
            }
            PlayerPrefs.SetInt(islandName + "Open",1);
        }
    }
}