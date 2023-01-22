using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Puzzle
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler,IEndDragHandler
    {
        public static event Action<Transform> BeginDragEvent; 
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Canvas canvas;
        [SerializeField] private CanvasGroup canvasGroup;
        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            canvasGroup.blocksRaycasts = false;
            if(transform.parent is null)
                return;
            BeginDragEvent?.Invoke(transform);
            eventData.pointerDrag.GetComponent<IBeginDrag>().BeginDrag();
        }

        public void OnDrag(PointerEventData eventData)
        {
            var newPos = rectTransform.anchoredPosition + eventData.delta / canvas.scaleFactor;
            rectTransform.anchoredPosition =newPos ;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            eventData.pointerDrag.GetComponent<IDroppable>().Dropped(false);
            canvasGroup.blocksRaycasts = true;
        }
    }
}