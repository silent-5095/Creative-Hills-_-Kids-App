using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WordsTable
{
    [CreateAssetMenu(menuName = "DataBase/Word Table/LetterButtonData", fileName = "LBData")]
    public class LetterButtonData : ScriptableObject
    {
        [SerializeField] private List<WordProp> wordProps;

        public WordProp GetProp(string value)
        {
            return wordProps.FirstOrDefault(wordProp => wordProp.value.Contains(value));
        }
    }
    
     [Serializable]
     public class WordProp
     {
         [SerializeField]
         private string name;
         public string value;
         public Sprite sprite;
         public bool isActive;
     }
}
