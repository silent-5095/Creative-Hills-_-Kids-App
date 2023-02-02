using System;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
{
    public class PuzzleController : MonoBehaviour
    {
        [SerializeField] private List<Drop> itemSlots;
        [SerializeField] private AudioSource clickSource,musicSource;
        [SerializeField] private AudioClip winClip;
        [SerializeField] private GameObject endPanel;
        private int _filledCount;

        private void OnDestroy()
        {           
            PuzzleEndPanel.EndPanelEvent -= OnEndPanelEvent;
        }

        private void Start()
        {
            PuzzleEndPanel.EndPanelEvent += OnEndPanelEvent;
            foreach (var slot in itemSlots)
            {
                slot.FillSlotEvent += OnFillSlotEvent;
            }
        }

        private void OnEndPanelEvent()
        {
            musicSource.Stop();
            musicSource.PlayOneShot(winClip);
        }

        private void OnFillSlotEvent()
        {
            _filledCount++;
            if (_filledCount >= itemSlots.Count)
                endPanel.GetComponent<PuzzleEndPanel>().ShowMainPanel();
            // winPanel.SetActive(true);
            else
                clickSource.Play();
        }
    }
}