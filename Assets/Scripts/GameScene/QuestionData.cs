using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    [CreateAssetMenu(menuName = "DataBase/Game Scene/QData_")]
    public class QuestionData : ScriptableObject
    {
        [SerializeField] private int qId;
        private int _islandButtonIndex;
        [SerializeField] private string summary;
        [SerializeField] private QuestionLevel level;
        public QuestionLevel Level => level;
        [SerializeField] private OptionProp option0, option1, option2, option3;

        public bool IsOpen
        {
            get => PlayerPrefs.GetInt(summary + "IsOpen") > 0;
            set => PlayerPrefs.SetInt(summary + "IsOpen", value ? 1 : 0);
        }

        public string GetSummary() => summary;

        public List<OptionProp> GetOptions()
        {
            var options = new List<OptionProp> {option0, option1, option2, option3};
            return options;
        }

        public bool IsCompleted
        {
            get => PlayerPrefs.GetInt(summary, 0) > 0;
            set => PlayerPrefs.SetInt(summary, value ? 1 : 0);
        }

        public int QuestionId
        {
            get => qId;
            set => qId=value;
        }
    }

    [Serializable]
    public class OptionProp
    {
        [SerializeField] private string value;
        [SerializeField] private bool isCorrect;

        public KeyValuePair<string, bool> GetOption()
        {
            return new KeyValuePair<string, bool>(value, isCorrect);
        }
    }

    public enum QuestionLevel
    {
        Easy,
        Medium,
        Hard
    }
}