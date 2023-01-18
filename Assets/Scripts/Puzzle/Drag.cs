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
        private Vector3 _delta;
        [SerializeField] private Canvas canvas;
        [SerializeField] private CanvasGroup canvasGroup;
        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            _delta = rectTransform.sizeDelta;
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("on begin drag");
            canvasGroup.blocksRaycasts = false;
            if(transform.parent is null)
                return;
            BeginDragEvent?.Invoke(transform);
            eventData.pointerDrag.GetComponent<IBeginDrag>().BeginDrag();
        }

        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log("onDrag");
            var newPos = rectTransform.anchoredPosition + eventData.delta / canvas.scaleFactor;
            rectTransform.anchoredPosition =newPos ;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("on end drag");
            eventData.pointerDrag.GetComponent<IDroppable>().Dropped(false);
            canvasGroup.blocksRaycasts = true;
        }
    }
}