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
        public static event Action<bool> QPanelRayCastEvent;

        [SerializeField] private GameObject mainPanel, correctPanel, wrongPanel;

        [SerializeField] private TextMeshProUGUI summary;
        [SerializeField] private ArabicFixerTMPRO fixerSummary;
        [SerializeField] private OptionButton[] optionButtons;
        [SerializeField] private AudioSource source;
        [SerializeField] private AudioClip[] aClips;
        private int _aClipIndex = 0;
        private QuestionData _currentData;
        private bool _answerIsCorrect;
        private Vector2 _defPos;

        private void OnDestroy()
        {
            Island.IslandRefreshQPanelEvent -= SetNewQuestion;
            GameSceneManager.RefreshDataEvent -= SetNewQuestion;
        }

        private void Start()
        {
            _defPos = mainPanel.transform.localPosition;
            Island.IslandRefreshQPanelEvent += SetNewQuestion;
            GameSceneManager.RefreshDataEvent += SetNewQuestion;
            foreach (var optionButton in optionButtons)
            {
                optionButton.ButtonClickEvent += OnOptionButtonClickEvent;
            }

            // mainPanel.SetActive(false);
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

            mainPanel.transform.localPosition = Vector2.zero;
            
            source.PlayOneShot(aClips[_aClipIndex]);
            _aClipIndex++;
            _aClipIndex = aClips.Length <= _aClipIndex ? 0 : _aClipIndex;
            
            QPanelRayCastEvent?.Invoke(true);
            // mainPanel.SetActive(true);
        }

        public void OnSubmitButton()
        {
            // mainPanel.SetActive(false);
            mainPanel.transform.localPosition = _defPos;
            QPanelRayCastEvent?.Invoke(false);

            Debug.Log("Answer event");
            AnswerEvent?.Invoke(_currentData, _answerIsCorrect);
        }

        public void OnCancelButton()
        {
            CancelQuestionEvent?.Invoke();
            mainPanel.transform.localPosition = _defPos;
            QPanelRayCastEvent?.Invoke(false);
            // mainPanel.SetActive(false);
        }
    }
}