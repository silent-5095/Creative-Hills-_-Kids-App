using System;
using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene
{
    public class QuestionPanel : MonoBehaviour
    {
        public static event Action<QuestionData> AnswerEvent;
        public static QuestionPanel Instance;
        [SerializeField] private GameObject mainPanel;
        [SerializeField] private RTLTextMeshPro summary;
        [SerializeField] private OptionButton[] optionButtons;
        [SerializeField] private Button[] refreshButtons;
        private QuestionData _currentData;

        private void Start()
        {
            OptionButton.ButtonClickEvent += OnOptionButtonClickEvent;
            
            mainPanel.SetActive(false);
            
            if (Instance is not null)
                Destroy(Instance);
            Instance = this;
            
            foreach (var refreshButton in refreshButtons)
            {
                refreshButton.onClick.AddListener(NextQuestion);
            }

        }

        private void OnOptionButtonClickEvent(OptionProp prop)
        {
            if (_currentData is not null)
            {
                _currentData.IsCompleted = prop.GetOption().Value;
                AnswerEvent?.Invoke(_currentData);
            }
        }

        public void SetNewQuestion(QuestionData data)
        {
            if (data is null)
                return;
            _currentData = data;
            Debug.Log(_currentData.GetSummary());
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
            // var newData = GameSceneManager.Instance.GetNextQuestion(_currentData);
            // SetNewQuestion(newData);
        }
    }
}