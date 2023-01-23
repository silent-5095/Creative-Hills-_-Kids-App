using System;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
{
    public class PuzzleController : MonoBehaviour
    {
        public static PuzzleController Instance;
        [SerializeField] private List<Drop> itemSlots;
        [SerializeField] private AudioSource source;
        [SerializeField] private GameObject winPanel;
        private int _filledCount;

        private void Start()
        {
            Instance = this;
            foreach (var slot in itemSlots)
            {
                slot.FillSlotEvent += OnFillSlotEvent;
            }
        }

        private void OnFillSlotEvent()
        {
            _filledCount++;
            if (_filledCount >= itemSlots.Count)
                winPanel.SetActive(true);
            else
                source.Play();
        }
    }
}