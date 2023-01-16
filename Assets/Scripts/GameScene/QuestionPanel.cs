using System;
using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene
{
    public class QuestionPanel : MonoBehaviour
    {
        public static event Action<QuestionData, bool> AnswerEvent;
        public static QuestionPanel Instance;
        [SerializeField] private GameObject mainPanel, correctPanel, wrongPanel;
        [SerializeField] private Button wrongPanelRefreshButton;
        [SerializeField] private RTLTextMeshPro summary;
        [SerializeField] private OptionButton[] optionButtons;
        private QuestionData _currentData;
        private bool _answerIsCorrect = false;

        private void OnDestroy()
        {
            Island.IslandRefreshQPanelEvent -= OnIslandRefreshQPanelEvent;
            OptionButton.ButtonClickEvent -= OnOptionButtonClickEvent;
        }

        private void Start()
        {
            Island.IslandRefreshQPanelEvent += OnIslandRefreshQPanelEvent;
            OptionButton.ButtonClickEvent += OnOptionButtonClickEvent;

            mainPanel.SetActive(false);

            if (Instance is not null)
                Destroy(Instance);
            Instance = this;

            wrongPanelRefreshButton.onClick.AddListener(NextQuestion);
            // foreach (var refreshButton in refreshButtons)
            // {
            //     refreshButton.onClick.AddListener(NextQuestion);
            // }
        }

        private void OnIslandRefreshQPanelEvent(QuestionData questionData)
        {
            _currentData = questionData;
        }

        private void OnOptionButtonClickEvent(OptionProp prop)
        {
            if (_currentData is null) return;
            _answerIsCorrect = prop.GetOption().Value;
            _currentData.IsCompleted = _currentData.IsCompleted || _answerIsCorrect;
            wrongPanel.SetActive(!_answerIsCorrect);
            correctPanel.SetActive(_answerIsCorrect);
            AnswerEvent?.Invoke(_currentData, _answerIsCorrect);
        }

        public void OnPopUpPanelSubmitButton()
        {
            // AnswerEvent?.Invoke(_currentData, _answerIsCorrect);
        }

        public void SetNewQuestion(QuestionData data)
        {
            if (data is null)
                return;
            _currentData = data;
            summary.text = _currentData.GetSummary();

            var options = data.GetOptions();
            for (var index = 0; index < options.Count; index++)
            {
                var option = options[index];
                optionButtons[index].SetData(option);
            }

            mainPanel.SetActive(true);
        }

        private void NextQuestion()
        {
            SetNewQuestion(_currentData);
        }
    }
}