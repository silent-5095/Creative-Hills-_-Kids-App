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
        [SerializeField] private Color fillColor, emptyColor;
        private Image _itemImage,_image;
        private Drop _drop;

        private void Awake()
        {
            _image = rect.GetComponent<Image>();
            _itemImage = item.GetComponent<Image>();
            _itemImage.sprite = sprite;
            _itemImage.SetNativeSize();
            _image.color = emptyColor;
            collider.size = rect.sizeDelta / 2;
            _drop = this;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject != item.gameObject) return;
            item.transform.SetParent(transform);
            item.localPosition = Vector2.zero;
            item.GetComponent<IDroppable>().Dropped(true); 
            _image.color =  fillColor;
            _drop.enabled =false;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag == item.gameObject)
            {
                item.transform.SetParent(transform);
                item.localPosition = Vector2.zero;
            }

            eventData.pointerDrag.GetComponent<IDroppable>().Dropped(item.gameObject == eventData.pointerDrag);
            _drop.enabled = item.gameObject != eventData.pointerDrag;
            _image.color = !_drop.enabled ? fillColor :emptyColor ;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("On Pointer enter in drop");
        }
    }
}