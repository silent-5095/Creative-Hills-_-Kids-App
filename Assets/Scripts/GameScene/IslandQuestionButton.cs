using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene
{
    public class IslandQuestionButton : MonoBehaviour
    {
        [SerializeField] private Button lockButton, openButton, passedButton;
        public QuestionData questionData;

        private void Start()
        {
            lockButton.onClick.AddListener(LockButtonOnClick);
            openButton.onClick.AddListener(OpenButtonOnClick);
            passedButton.onClick.AddListener(PassedButtonOnClick);
        }

        public void Lock()
        {
            ResetButton();
            Debug.Log("Lock");
            lockButton.gameObject.SetActive(true);
        }

        public void Open()
        {
            ResetButton();
            Debug.Log("Open");
            openButton.gameObject.SetActive(true);
        }

        public void Pass()
        {
            ResetButton();
            Debug.Log("Pass");
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
            QuestionPanel.Instance.SetNewQuestion(questionData);
        }

        private void PassedButtonOnClick()
        {
            // select Passed Question
            QuestionPanel.Instance.SetNewQuestion(questionData);
        }
    }
}