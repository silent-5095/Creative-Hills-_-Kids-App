using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Puzzle
{
    public class PuzzleEndPanel : MonoBehaviour
    {
        public static event Action EndPanelEvent;
        [SerializeField] private Image mainPanel;
        [SerializeField] private Image[] currentEndPanel;
        [SerializeField] private int currentIndex;
        public void ShowMainPanel()
        {
            mainPanel.gameObject.SetActive(true);
            var mySequence = DOTween.Sequence();
            var tween= mainPanel.DOFade(0.4f, 0.2f);
           tween.onComplete += () =>
           {
               currentEndPanel[currentIndex].gameObject.SetActive(true);
               
           };
           var tween2=currentEndPanel[currentIndex].DOFade(1, 0.2f);
           tween2.onPlay += () =>
           {
               var tween3 = currentEndPanel[currentIndex].transform.DORotate(Vector3.zero, 0.3f);
               EndPanelEvent?.Invoke();
           };
           
           mySequence.Append(tween);
           mySequence.Append(tween2);

        }
    }
}
