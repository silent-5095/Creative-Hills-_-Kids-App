using System;
using RTLTMPro;
using TMPro;
using UnityEngine;

namespace GameScene
{
    public class QuestionPanel : MonoBehaviour
    {
        public static event Action<QuestionData, bool> AnswerEvent;
        public static event Action CancelQuestionEvent;

        [SerializeField] private GameObject mainPanel, correctPanel, wrongPanel;

        // [SerializeField] private RTLTextMeshPro summary;
        [SerializeField] private TextMeshProUGUI summary;
        [SerializeField] private ArabicFixerTMPRO fixerSummary;
        [SerializeField] private OptionButton[] optionButtons;
        private QuestionData _currentData;
        private bool _answerIsCorrect;

        private void OnDestroy()
        {
            Island.IslandRefreshQPanelEvent -= SetNewQuestion;
        }

        private void Start()
        {
            Island.IslandRefreshQPanelEvent += SetNewQuestion;
            foreach (var optionButton in optionButtons)
            {
                optionButton.ButtonClickEvent += OnOptionButtonClickEvent;
            }

            mainPanel.SetActive(false);
        }

        private void OnOptionButtonClickEvent(OptionProp prop)
        {
            if (_currentData is null) return;
            _answerIsCorrect = prop.GetOption().Value;
            _currentData.IsCompleted = _currentData.IsCompleted || _answerIsCorrect;
            wrongPanel.SetActive(!_answerIsCorrect);
            correctPanel.SetActive(_answerIsCorrect);
        }

        private void SetNewQuestion(QuestionData data)
        {
            if (data is null)
                return;
            _currentData = data;
            // summary.text = _currentData.GetSummary();
            fixerSummary.fixedText = _currentData.GetSummary();

            var options = data.GetOptions();
            Debug.Log(data.GetOptions().Count);
            for (var index = 0; index < options.Count; index++)
            {
                optionButtons[index].gameObject.SetActive(true);
                var option = options[index];
                optionButtons[index].SetData(option);
            }

            if (optionButtons.Length > options.Count)
            {
                for (var i = options.Count; i < optionButtons.Length; i++)
                {
                    optionButtons[i].gameObject.SetActive(false);
                }
            }

            mainPanel.SetActive(true);
        }

        public void OnSubmitButton()
        {
            mainPanel.SetActive(false);
            AnswerEvent?.Invoke(_currentData, _answerIsCorrect);
        }

        public void OnCancelButton()
        {
            CancelQuestionEvent?.Invoke();
            mainPanel.SetActive(false);
        }
    }
}