using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Shop
{
    public class ShopSlot : MonoBehaviour
    {
        [SerializeField] private List<ShopItem> items;
        public static event Action<bool> DropEvent;
        private bool _pointerEntered;
        private ShopItem _currentItem;

        private void Start()
        {
            items = FindObjectsOfType<ShopItem>().ToList();
            foreach (var item in items)
            {
                item.EndDragEvent += ItemOnEndDragEvent;
            }
        }

        private void ItemOnEndDragEvent(ShopItem item)
        {
            _currentItem = item;
            item.Reset();
        }

        public void Drop()
        {
            if (_currentItem != null && _currentItem.IsActive)
            {
                _currentItem.AttachToSlot();
            }
            DropEvent?.Invoke(_currentItem != null && _currentItem.IsActive);
        }
    }
}