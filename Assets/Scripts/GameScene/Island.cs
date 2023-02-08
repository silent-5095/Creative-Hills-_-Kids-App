using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene
{
    public class Island : MonoBehaviour
    {
        public static event Action<QuestionData> IslandRefreshQPanelEvent;
        public static event Action<Transform,IslandType> CompleteIslandEvent;
        // public static event Action<Transform> CompleteIslandGameEvent;
        public static string IslandGameRef;
        [SerializeField] private IslandType isLandType;
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

            isGamePassed = PlayerPrefs.GetInt(isLandType + "Game") > 0;
            if (IslandGameRef != isLandType.ToString() || isGamePassed) return;
            CompleteIslandEvent?.Invoke(nextIsland.transform, IslandType.Game);
            PlayerPrefs.SetInt(isLandType + "Game", 1);
            isGamePassed = true;
            IslandGameRef = string.Empty;
            if(nextIsland == null)
                return;
            nextIsland.OpenIsland();
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
            IslandGameRef = isLandType.ToString();
            ForDemo.Instance.LoadScene(gameSceneName);
        }

        public void OnQuestionAnswer()
        {
            isOpen = isFirst || PlayerPrefs.GetInt(isLandType + "Open") > 0;
            isPassed = PlayerPrefs.GetInt(isLandType + "Passed") > 0;
            gameLock.SetActive(!(PlayerPrefs.GetInt(isLandType + "GameLock") > 0));
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
            PlayerPrefs.SetInt(isLandType + "Passed", isPassed ? 1 : 0);
            PlayerPrefs.SetInt(isLandType + "GameLock", isPassed ? 1 : 0);
            if (!isPassed) return;
            CompleteIslandEvent?.Invoke(gameButton.transform,isLandType);
            gameLock.SetActive(false);
        }

        private void OpenIsland()
        {
            isOpen = true;
            foreach (var button in buttons)
            {
                button.HandleCondition();
            }
            PlayerPrefs.SetInt(isLandType + "Open",1);
        }
    }

    public enum IslandType
    {
        Mattrah,
        Sohar,
        Nizwa,
        Game
    }
}