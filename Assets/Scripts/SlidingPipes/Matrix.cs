using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace SlidingPipes
{
    public class Matrix : MonoBehaviour
    {
        public static event Action<bool> WinEvent; 
        [SerializeField] private Section[] sections;
        private List<Vector2> _changeOrderArr;
        [SerializeField] private List<SectionType> _correctPath;
        private int _currentIndex;
        private bool _isWin;

        #region OnValidate Section

#if UNITY_EDITOR
        // [SerializeField] private bool shuffle;
        [SerializeField] private bool saveCorrectPath;

        private void OnValidate()
        {
            // if (shuffle)
            // {
            //     shuffle = false;
            //     _changeOrderArr = new List<Vector2>();
            //     // sections = GetComponentsInChildren<Section>();
            //     foreach (var section in sections)
            //     {
            //         if (section.gameObject.activeSelf && section.SectionProp.type != SectionType.Start &&
            //             section.SectionProp.type != SectionType.End)
            //         {
            //             _changeOrderArr.Add(section.gameObject.transform.position);
            //         }
            //     }
            //
            //     ChangeOrder();
            // }
            if (saveCorrectPath)
            {
                _correctPath = null;
                _correctPath = new List<SectionType>();
                saveCorrectPath = false;
                foreach (var section in sections)
                {
                    _correctPath.Add(section.SectionProp.type);
                }

                Debug.Log("PathSaved!");
            }
        }

        private void ChangeOrder()
        {
            foreach (var t in sections)
            {
                if (!t.gameObject.activeSelf || t.SectionProp.type == SectionType.End ||
                    t.SectionProp.type == SectionType.Start) continue;
                var rand = Random.Range(0, _changeOrderArr.Count);
                var pos = _changeOrderArr[rand];
                _changeOrderArr.RemoveAt(rand);
                t.transform.position = pos;
            }
        }
#endif

        #endregion

        private void OnDestroy()
        {
            Section.MoveEvent -= OnMoveCheck;
        }

        private void Start()
        {
            // sections = GetComponentsInChildren<Section>();
            Section.MoveEvent += OnMoveCheck;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Check Button");
                OnMoveCheck();
            }
        }

        private void OnMoveCheck()
        {
            _isWin = false;
            for (var index = 0; index < sections.Length; index++)
            {
                var section = sections[index];
                if (_correctPath[index] == SectionType.None || _correctPath[index]== SectionType.Movable) continue;
                _isWin = _correctPath[index] == section.SectionProp.type;
                Debug.Log($"OnMove CHeck and isWin is ={_isWin}");
                if (!_isWin) break;
            }
            if (_isWin)
                Win();
            else
            {
                Debug.Log("InCorrect sort");
            }
        }

        private void Win()
        {
            WinEvent?.Invoke(true);
            Debug.Log("Win");
        }
    }
}