using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameScene
{
    public class GameSceneManager : MonoBehaviour
    {
        public static GameSceneManager Instance;
        [SerializeField] private List<QuestionData> questionDataList;
        private List<QuestionData> remainList, passedList;
        private int _remainQIndex, _passedQIndex;
        public int TotalQuestion() => questionDataList.Count;

        private void OnDestroy()
        {
            QuestionPanel.AnswerEvent -= OnAnswerEvent;
        }

        private void Awake()
        {
            QuestionPanel.AnswerEvent += OnAnswerEvent;
            if (Instance is not null)
                Destroy(Instance);
            Instance = this;
            Debug.Log("dd");
            passedList=new List<QuestionData>();
            remainList = questionDataList.ToList();
            foreach (var questionData in questionDataList)
            {
                if (questionData.IsCompleted)
                {
                    passedList.Add(questionData);
                    remainList.Remove(questionData);
                }
            }

            remainList[0].IsOpen = true;
        }


        private void OnAnswerEvent(QuestionData arg1, bool arg2)
        {
            if (!arg2) return;
            if (remainList.Contains(arg1))
                remainList.Remove(arg1);
            if (passedList.Contains(arg1))
                return;
            passedList.Add(arg1);
        }

        public QuestionData GetNextRemain(QuestionData currentQuestion)
        {
            var cIndex = remainList.FindIndex(c => c == currentQuestion);
            cIndex = remainList.Count - 1 > cIndex ? cIndex + 1 : 0;
            return remainList[cIndex] is null ? null : remainList[cIndex];
        }

        public QuestionData GetNextPassed(QuestionData currentQuestion)
        {
            var cIndex = passedList.FindIndex(c => c == currentQuestion);
            cIndex = passedList.Count - 1 == cIndex ? 0 : cIndex + 1;
            return passedList[cIndex] is null ? null : passedList[cIndex];
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