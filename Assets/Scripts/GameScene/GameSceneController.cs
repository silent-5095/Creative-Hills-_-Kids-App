using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    public class GameSceneController : MonoBehaviour
    {
        [SerializeField] private List<QuestionData> questionDataList;
        [HideInInspector]
        [SerializeField]private List<QuestionData> easyQuestions, mediumQuestions, hardQuestions;

        private void Start()
        {
            foreach (var questionData in questionDataList)
            {
                switch (questionData.Level)
                {
                    case QuestionLevel.Easy:
                        easyQuestions.Add(questionData);
                        break;
                    case QuestionLevel.Medium:
                        mediumQuestions.Add(questionData);
                        break;
                    case QuestionLevel.Hard:
                        hardQuestions.Add(questionData);
                        break;
                    default:
                        easyQuestions.Add(questionData);
                        break;
                }
            }
        }

        public QuestionData GetNextQuestion(QuestionLevel level)
        {
            switch (level)
            {
                case QuestionLevel.Easy:
                    break;
                case QuestionLevel.Medium:
                    break;
                case QuestionLevel.Hard:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }

            return null;
        }
        
    }
}