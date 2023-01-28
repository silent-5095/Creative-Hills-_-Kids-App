using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace GameScene
{
    public class GameSceneManager : MonoBehaviour
    {
        public static GameSceneManager Instance;
        public static event Action<QuestionData> RefreshDataEvent;
        [SerializeField] private List<IslandQuestionButton> islandButtons;
        [SerializeField] private List<QuestionData> questionDataList;
        [SerializeField] private Island[] islands;
        private List<QuestionData> _remainList, _passedList;
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
            SplitData();
        }

        private void SplitData()
        {
            string passedString;
            passedString = PlayerPrefs.GetString(nameof(passedString));
            _passedList = new List<QuestionData>();
            var tempList = JsonConvert.DeserializeObject<List<QuestionData>>(passedString);
            if (tempList is not null)
                foreach (var tQuestionData in tempList)
                {
                    _passedList.Add(questionDataList.Find(q => q.QuestionId == tQuestionData.QuestionId));
                    questionDataList.Remove(_passedList[^1]);
                }
            //
             _remainList = questionDataList;
              for (int i = 0; i < questionDataList.Count; i++)
              {
                  if (_passedList.Contains(questionDataList[i]))
                  {
                      questionDataList[i].IsOpen = true;
                      questionDataList[i].IsCompleted = true;
                      _remainList.Remove(questionDataList[i]);
                  }
                  else
                  {
                      questionDataList[i].IsOpen = false;
                      questionDataList[i].IsCompleted = false;
                  }
              }

            _remainList[0].IsOpen = true;
            Debug.Log(_remainList[0].IsOpen);
            SetButtonData();
        }

        private void SetButtonData()
        {
            for (int i = 0; i < _passedList.Count; i++)
            {
                islandButtons[i].questionData = _passedList[i];
            }

            int j = 0;
            for (int i = _passedList.Count; i < islandButtons.Count; i++)
            {
                islandButtons[i].questionData = _remainList[j];
                j++;
            }

            foreach (var island in islands)
            {
                island.Prepare();
            }
        }

        private void SaveData()
        {
            var passedString = JsonConvert.SerializeObject(_passedList);
            PlayerPrefs.SetString(nameof(passedString), passedString);
        }


        private void OnAnswerEvent(QuestionData data, bool isCorrect)
        {
            if (!isCorrect)
            {
                if (_passedList.Contains(data))
                {
                    ShiftPq();
                    RefreshDataEvent?.Invoke(_passedList[0]);
                }
                else
                {
                    ShiftRq();
                    RefreshDataEvent?.Invoke(_remainList[0]);
                }

                Debug.Log("On Answer event");
                SetButtonData();
                SaveData();
                return;
            }

            _passedList ??= new List<QuestionData>();
            if (_passedList.Contains(data))
                return;
            Debug.Log("On Answer Correct");
            _passedList.Add(data);
            Debug.Log(_remainList[0].name);
            _remainList.RemoveAt(0);
            Debug.Log(_remainList[0].name);
            _remainList[0].IsOpen = true;
            SetButtonData();
            SaveData();
        }


        public void ShiftRq()
        {
            Debug.Log("Shift");
            var newQ = _remainList[0];
            newQ.IsOpen = false;
            Debug.Log(newQ.name);
            _remainList.Remove(newQ);
            _remainList.Add(newQ);
            _remainList[0].IsOpen = true;
            Debug.Log(_remainList[0].name);
        }

        public void ShiftPq()
        {
            var newQ = _passedList[0];
            newQ.IsOpen = false;
            _passedList.Remove(newQ);
            _passedList.Add(newQ);
        }

        public QuestionData GetNextPassed(QuestionData currentQuestion)
        {
            var cIndex = _passedList.FindIndex(c => c == currentQuestion);
            cIndex = _passedList.Count - 1 == cIndex ? 0 : cIndex + 1;
            return _passedList[cIndex] is null ? null : _passedList[cIndex];
        }

        // public QuestionData GetQuestion(int index)
        // {
        //     return questionDataList.Count <= index ? null : questionDataList[index];
        // }

        // public QuestionData GetQuestionById(int id)
        // {
        //     return questionDataList.Find(q => q.QuestionId == id);
        // }
    }
}