using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace SlidingPipes
{
    public class Matrix : MonoBehaviour
    {
        public static event Action<bool> WinEvent;
        [SerializeField] private AudioSource source;
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
            Section.MoveEvent += OnMoveCheck;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnMoveCheck();
            }
        }

        private void OnMoveCheck()
        {
            _isWin = false;
            for (var index = 0; index < sections.Length; index++)
            {
                var section = sections[index];
                if (_correctPath[index] == SectionType.None || _correctPath[index] == SectionType.Movable) continue;
                _isWin = _correctPath[index] == section.SectionProp.type;
                if (!_isWin) break;
            }

            source.Play();
            if (_isWin)
                Win();
            // else
            // {
            //     Debug.Log("InCorrect sort");
            // }
        }

        private void Win()
        {
             WinEvent?.Invoke(true);
        }

    }
}