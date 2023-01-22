using System;
using UnityEngine;

namespace SlidingPipes
{
    public class MsgBox : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private bool startPos;

        private void OnDestroy()
        {
            AspectDetector.AspectDetectorEvent -= OnAspectDetectorEvent;
        }

        private void Awake()
        {
            AspectDetector.AspectDetectorEvent += OnAspectDetectorEvent;
        }

        public bool update;

        private void OnValidate()
        {
            if (update)
            {
                update = false;
                Debug.Log(target.localPosition);
                Debug.Log(target.position);
                Debug.Log(transform.localPosition);
                Debug.Log(transform.position);
                Debug.Log(GetComponent<Renderer>().bounds.size);
                Debug.Log(target.GetComponent<Renderer>().bounds.size);
                Debug.Log(transform.position - target.position);
                // Debug.Log(GetComponent<RectTransform>().position);
                // Debug.Log(GetComponent<RectTransform>().localPosition);
                // Debug.Log(GetComponent<RectTransform>().anchoredPosition);
                // Debug.Log(GetComponent<RectTransform>().anchoredPosition3D);
            }
        }

        private void OnAspectDetectorEvent(bool isTablet)
        {
            // var bound = GetComponent<Renderer>().bounds.size;
            // transform.position =
            //     new Vector3(startPos
            //         ? target.position.x + bound.x
            //         : target.position.x - bound.x, target.position.y);
        }
    }
}