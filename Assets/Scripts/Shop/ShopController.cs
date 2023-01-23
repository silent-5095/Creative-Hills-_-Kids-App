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
        [SerializeField] private GameObject winPanel;
        [SerializeField] private AudioSource source;

        private void Start()
        {
            items = FindObjectsOfType<ShopItem>().ToList();
            foreach (var item in items)
            {
                item.AttachEvent += OnAttachEvent;
                if (item.IsActive)
                    _activeItemCount++;
            }
        }

        private void OnAttachEvent()
        {
            _activeItemCount--;
            if (_activeItemCount <= 0)
                Win();
            else
                source.Play();
        }

        private void Win()
        {
            winPanel.SetActive(true);
        }
    }
}