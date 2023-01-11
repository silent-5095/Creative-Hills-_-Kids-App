using System;
using System.Collections.Generic;
using Extension;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene
{
    public class QuestionPanel : MonoBehaviour
    {
        public static QuestionPanel Instance;
        [SerializeField] private GameObject mainPanel;
        [SerializeField] private TextMeshProUGUI summary;
        [SerializeField] private OptionButton[] optionButtons;

        private void Start()
        {
            if (Instance is not null)
                Destroy(Instance);
            Instance = this;
            foreach (var optionButton in optionButtons)
            {
                optionButton.ButtonClickEvent += OnOptionButtonClickEvent;
            }
        }

        private void OnOptionButtonClickEvent(OptionProp prop)
        {
            // Disable all Button and Display Correct Answer
            PlayerPrefs.SetString(summary.text, prop.GetOption().Key);
            foreach (var button in optionButtons)
            {
                button.IsCompleted(button.OptionProp == prop);
            }
        }

        public void SetNewQuestion(QuestionData data)
        {
            mainPanel.SetActive(true);
            summary.text = data.GetSummary();
            var isCompleted = data.IsCompleted;

            var options = data.GetOptions();
            for (var index = 0; index < options.Count; index++)
            {
                var option = options[index];
                optionButtons[index].SetData(option);
                optionButtons[index].IsCompleted(isCompleted);
            }
        }
    }
}