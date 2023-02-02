using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shop
{
    public class ShopController : MonoBehaviour
    {
        [SerializeField] private List<ShopItem> items;
        [SerializeField] private string sceneName;
        private int _activeItemCount = 0;
        [SerializeField] private WinPanel winPanel;
        [SerializeField] private AudioSource source;
        [SerializeField] private AudioClip wrongAClip, correctAClip;

        private void OnDestroy()
        {
            ShopSlot.DropEvent -= OnEndDragEvent;
        }

        private void Start()
        {
            items = FindObjectsOfType<ShopItem>().ToList();
            foreach (var item in items)
            {
                item.AttachEvent += OnAttachEvent;
                if (item.IsActive)
                    _activeItemCount++;
            }

            ShopSlot.DropEvent += OnEndDragEvent;
        }

        private void OnAttachEvent()
        {
            _activeItemCount--;
            if (_activeItemCount <= 0)
                Win();
            else
                source.PlayOneShot(correctAClip);
        }

        private void OnEndDragEvent(bool con)
        {
            if (!con)
                source.PlayOneShot(wrongAClip);
        }

        private void Win()
        {
            winPanel.ShowWinPanel();
        }
    }
}