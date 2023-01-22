using System;
using System.Collections.Generic;
using UnityEngine;

namespace Magazine
{
    public class MagazineSection : MonoBehaviour
    {
        [SerializeField] private List<MagazineData> dataList;
        [SerializeField] private Transform contentHolder;
        [SerializeField] private MagazineSelector mSelectorPrefab;

        private void Start()
        {
            foreach (var data in dataList)
            {
                var selector = Instantiate(mSelectorPrefab, contentHolder);
                selector.SetData(data);
                selector.SelectorEvent += OnSelectorEvent;
            }
        }

        private void OnSelectorEvent(MagazineData selectedData)
        {
            
        }
    }
}
