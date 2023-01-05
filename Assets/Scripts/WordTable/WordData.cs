using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WordTable
{
    [CreateAssetMenu(menuName = "DataBase/Word Table/WordData", fileName = "WData")]
    public class WordData : ScriptableObject
    {
        [SerializeField] private List<LetterData> wordProps;

        public LetterData GetProp(string value)
        {
            return wordProps.FirstOrDefault(wordProp => wordProp.value.Contains(value));
        }
    }
    //
    // [Serializable]
    // public class WordProp
    // {
    //     [SerializeField]
    //     private string name;
    //     public string[] value;
    //     public Sprite sprite;
    // }
}
