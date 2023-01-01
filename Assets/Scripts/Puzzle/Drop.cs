using System;
using DG.Tweening;
using UnityEngine;

namespace Puzzle
{
    public class Drop : MonoBehaviour
    {
        [SerializeField] private Drag target;
        [SerializeField] private float offset;
        [SerializeField] private SpriteRenderer renderer;
        [SerializeField] private Color color;
        private int _sortOrder = 0;

        public void SetSprite(SpriteRenderer sp)
        {
            renderer.sprite = sp.sprite;
            _sortOrder = sp.sortingOrder;
            renderer.sortingOrder = _sortOrder;
            // renderer.sortingOrder = 0;
            // renderer.sortingLayerID = sp.sortingLayerID;
        }

        private void Start()
        {
            target.DropEvent += DropEvent;
        }

        private void DropEvent()
        {
            var targetPos = target.transform.position;
            var pos = transform.position;
            if (!(Mathf.Abs(targetPos.x - pos.x) <= offset) || !(Mathf.Abs(targetPos.y - pos.y) <= offset) ||
                !(Mathf.Abs(targetPos.z - pos.z) <= offset)) return;
            var tween = target.transform.DOMove(transform.position, 0.1f);
            tween.onComplete += () => tween.Kill();
            target.IsPlaced = true;
            renderer.color = color;
            renderer.sortingOrder = _sortOrder;
        }
    }
}