using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene
{
    public class IslandQuestionButton : MonoBehaviour
    {
        public event Action<IslandQuestionButton> OnButtonClickEvent;
        [SerializeField] private Button lockButton, openButton, passedButton;
        public QuestionData questionData;

        private void Start()
        {
            lockButton.onClick.AddListener(LockButtonOnClick);
            openButton.onClick.AddListener(OpenButtonOnClick);
            passedButton.onClick.AddListener(PassedButtonOnClick);
        }

        public void HandleCondition()
        {
            if (questionData is null)
            {
                Lock();
                return;
            }

            if (questionData.IsCompleted)
            {
                Pass();
                return;
            }

            if (questionData.IsOpen)
            {
                Open();
            }
            else
            {
                Lock();
            }
        }

        public void Lock()
        {
            ResetButton();
            lockButton.gameObject.SetActive(true);
        }

        public void Open()
        {
            ResetButton();
            openButton.gameObject.SetActive(true);
        }

        public void Pass()
        {
            ResetButton();
            passedButton.gameObject.SetActive(true);
        }

        private void ResetButton()
        {
            lockButton.gameObject.SetActive(false);
            openButton.gameObject.SetActive(false);
            passedButton.gameObject.SetActive(false);
        }

        private void LockButtonOnClick()
        {
            //Pop Up Pass Previous Question
        }

        private void OpenButtonOnClick()
        {
            // select remain Question
            OnButtonClickEvent?.Invoke(this);
        }

        private void PassedButtonOnClick()
        {
            // select Passed Question
            OnButtonClickEvent?.Invoke(this);
        }
    }
}