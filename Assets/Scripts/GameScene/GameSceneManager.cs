using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    public class GameSceneManager : MonoBehaviour
    {
        public static GameSceneManager Instance;
        [SerializeField] private List<QuestionData> questionDataList;
        [HideInInspector] [SerializeField] private List<QuestionData> remainList, passedList;

        public int TotalQuestion() => questionDataList.Count;
        private void Awake()
        {
            QuestionPanel.AnswerEvent+= OnAnswerEvent;
            if (Instance is not null)
                Destroy(Instance);
            Instance = this;
            foreach (var questionData in questionDataList)
            {
                if (questionData.IsCompleted)
                    passedList.Add(questionData);
                else
                    remainList.Add(questionData);
            }
        }

        private void OnAnswerEvent(QuestionData arg1, bool arg2)
        {
            if (!arg2) return;
            if(passedList.Contains(arg1))
                return;
            passedList.Add(arg1);
        }

        public QuestionData GetNextRemain(QuestionData currentQuestion)
        {
            var cIndex=remainList.FindIndex(c => c == currentQuestion);
            cIndex = remainList.Count - 1 == cIndex ? 0 : cIndex;
            return remainList[cIndex + 1];
        }
        public QuestionData GetNextPassed(QuestionData currentQuestion)
        {
            var cIndex=passedList.FindIndex(c => c == currentQuestion);
            cIndex = passedList.Count - 1 == cIndex ? 0 : cIndex;
            return passedList[cIndex + 1];
        }
        public QuestionData GetQuestion(int index)
        {
            return questionDataList.Count <= index ? null : questionDataList[index];
        }

        public QuestionData GetQuestionById(int id)
        {
            return questionDataList.Find(q => q.QuestionId == id);
        }
    }
}