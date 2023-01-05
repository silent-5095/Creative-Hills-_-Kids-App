using UnityEngine;

namespace WordTable
{
    [CreateAssetMenu(menuName = "DataBase/Word Table/LetterData", fileName = "WLetter")]
    public class LetterData : ScriptableObject
    {
        [SerializeField]
        private string name;
        public string value;
        public Sprite sprite;
        public bool isActive;
    }
}

