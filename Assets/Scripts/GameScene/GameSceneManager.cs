using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameScene
{
    public class GameSceneManager : MonoBehaviour
    {
        public static GameSceneManager Instance;
        [SerializeField] private List<QuestionData> questionDataList;
        [HideInInspector] [SerializeField] private List<QuestionData> remainList, passedList;

        private void Awake()
        {
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

        public QuestionData GetNextQuestion(QuestionData currentQuestion)
        {
            while (true)
            {
                int index;
                if (currentQuestion.IsCompleted)
                {
                    index = Random.Range(0, passedList.Count);
                    var selectedQ = passedList[index];
                    if (selectedQ == currentQuestion) continue;
                    return selectedQ;
                }
                else
                {
                    index = Random.Range(0, remainList.Count);
                    var selectedQ = remainList[index];
                    if (selectedQ == currentQuestion) continue;
                    return selectedQ;
                }
            }
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


/*
 
 
                    Field
                    
        // [HideInInspector]
        // [SerializeField]private List<QuestionData> easyQuestions, mediumQuestions, hardQuestions;
 
 
 *                    in Start Method
 *        
            // foreach (var questionData in questionDataList)
            // {
            //     switch (questionData.Level)
            //     {
            //         case QuestionLevel.Easy:
            //             easyQuestions.Add(questionData);
            //             break;
            //         case QuestionLevel.Medium:
            //             mediumQuestions.Add(questionData);
            //             break;
            //         case QuestionLevel.Hard:
            //             hardQuestions.Add(questionData);
            //             break;
            //         default:
            //             easyQuestions.Add(questionData);
            //             break;
            //     }
            // }
 *
 *
 *
 *                 Replace GetNextQuestion
 *                 

        // public QuestionData GetNextQuestion(QuestionLevel level)
        // {
        //     switch (level)
        //     {
        //         case QuestionLevel.Easy:
        //             break;
        //         case QuestionLevel.Medium:
        //             break;
        //         case QuestionLevel.Hard:
        //             break;
        //         default:
        //             throw new ArgumentOutOfRangeException(nameof(level), level, null);
        //     }
        //
        //     return null;
        // }
 *
 * 
 */