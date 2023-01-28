using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shop
{
    public class ShopSlot : MonoBehaviour
    {
        [SerializeField] private List<ShopItem> items;
        [SerializeField] private AudioSource audioSource;
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
            audioSource.Play();
            _currentItem = item;
            item.Reset();
            // if (!item.IsActive)
            // {
            //      item.ResetPosition();
            // }
            // else
            // {
            //     _currentItem = item;
            // }
        }

        public void Drop()
        {
            if (_currentItem != null && _currentItem.IsActive)
                _currentItem.AttachToSlot();
        }
    }
}