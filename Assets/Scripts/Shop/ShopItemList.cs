using System;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class ShopItemList : MonoBehaviour
    {

        [SerializeField] private Image tick;

        private void Start()
        {
            tick.gameObject.SetActive(true);
        }
        public void ActiveTick()
        {
            tick.enabled = true;
        }
    }
}
