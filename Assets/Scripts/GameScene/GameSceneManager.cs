using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace GameScene
{
    public class GameSceneManager : MonoBehaviour
    {
        public static GameSceneManager Instance;
        public static event Action<QuestionData> RefreshDataEvent;
        public static event Action ChangeButtonDataEvent;
        [SerializeField] private List<IslandQuestionButton> islandButtons;
        [SerializeField] private List<QuestionData> questionDataList;
        [SerializeField] private Island[] islands;
        private List<QuestionData> _remainList, _passedList;
        private int _remainQIndex, _passedQIndex;
        private void OnDestroy()
        {
            QuestionPanel.AnswerEvent -= OnAnswerEvent;
        }
        private void Start()
        {
            QuestionPanel.AnswerEvent += OnAnswerEvent;
            SplitData();
            if (Instance is not null)
                Destroy(Instance);
            Instance = this;
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

            _remainList = questionDataList;
            for (var i = 0; i < questionDataList.Count; i++)
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
                // island.Prepare();
                Debug.Log("in game Scene Manager SetButton Data");
                island.OnQuestionAnswer();
            }
        }
        private void SaveData()
        {
            var passedString = JsonConvert.SerializeObject(_passedList);
            PlayerPrefs.SetString(nameof(passedString), passedString);
        }
        private void OnAnswerEvent(QuestionData data, bool isCorrect)
        {
            Debug.Log(isCorrect);
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
                SetButtonData();
                SaveData();
                return;
            }

            _passedList ??= new List<QuestionData>();
            if (_passedList.Contains(data))
                return;
            _passedList.Add(data);
            _remainList.RemoveAt(0);
            _remainList[0].IsOpen = true;
            SetButtonData();
            SaveData();
        }
        public void ShiftRq()
        {
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
    }
}