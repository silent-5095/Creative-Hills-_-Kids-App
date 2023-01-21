using System;
using UnityEngine;
using UnityEngine.UI;

namespace WordsTable
{
    public class WordTargetCell : MonoBehaviour
    {
        [SerializeField] private Image thickImg;
        [SerializeField] private string value;
        [SerializeField] private RTLTMPro.RTLTextMeshPro3D text;

        private void OnDestroy()
        {
            WordTableController.CompleteWordEvent -= WordTableControllerOnCompleteWordEvent;
        }

        private void Start()
        {
            WordTableController.CompleteWordEvent += WordTableControllerOnCompleteWordEvent;
            text.text = value;
        }

        private void WordTableControllerOnCompleteWordEvent(string targetValue)
        {
            if (value == targetValue)
                Active();
        }

        private void Active()
        {
            Debug.Log("Word Target is completed");
            thickImg.gameObject.SetActive(true);
        }
    }
}