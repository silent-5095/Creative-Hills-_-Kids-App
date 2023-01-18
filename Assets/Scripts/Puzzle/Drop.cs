using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Puzzle
{
    public class Drop : MonoBehaviour, IDropHandler, IPointerEnterHandler
    {
        [SerializeField] private RectTransform rect;
        [SerializeField] private new BoxCollider2D collider;
        [SerializeField] private RectTransform item;
        [SerializeField] private Sprite sprite;
        private Drop _drop;

        private void Awake()
        {
           var image= rect.GetComponent<Image>();
            var itemImage=item.GetComponent<Image>() ;
            image.sprite = sprite;
            itemImage.sprite = sprite;
            itemImage.SetNativeSize();
            collider.size = rect.sizeDelta/2;
            _drop = this;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject == item.gameObject)
            {
                item.transform.SetParent(transform);
                item.localPosition = Vector2.zero;
                item.GetComponent<IDroppable>().Dropped(true);
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            // Debug.Log($"On Drop {eventData.pointerDrag.name}");
            if (eventData.pointerDrag == item.gameObject)
            {
                item.transform.SetParent(transform);
                item.localPosition = Vector2.zero;
            }

            eventData.pointerDrag.GetComponent<IDroppable>().Dropped(item.gameObject == eventData.pointerDrag);
            _drop.enabled = item.gameObject != eventData.pointerDrag;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("On Pointer enter in drop");
        }
    }
}