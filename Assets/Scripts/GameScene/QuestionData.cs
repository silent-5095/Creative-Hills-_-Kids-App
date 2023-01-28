using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace GameScene
{
    [CreateAssetMenu(menuName = "DataBase/Game Scene/QData_")]
    public class QuestionData : ScriptableObject
    {
        [SerializeField] private int qId;
        private int _islandButtonIndex;
        [Multiline] [SerializeField] private string summary;
        [SerializeField] private QuestionLevel level;
        public QuestionLevel Level => level;

        [SerializeField] private OptionProp[] options;

        // [SerializeField] private OptionProp option0, option1, option2, option3;
        public bool IsOpen { get; set; }

        public string GetSummary() => summary;

        public List<OptionProp> GetOptions()
        {
            // var tempOptions = new List<OptionProp> {option0, option1, option2, option3};
            var tempOptions = options.ToList();
            return tempOptions;
        }

        public bool IsCompleted { get; set; }

        public int QuestionId
        {
            get => qId;
            set => qId = value;
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