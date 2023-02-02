using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    public class IslandManager : MonoBehaviour
    {
        [SerializeField] private List<Island> islands;
        [SerializeField] private IslandCameraController cameraController;
        [SerializeField] private IslandWinPanel winPanel;
        private Transform _currentTarget;

        private void OnDestroy()
        {
            Island.CompleteIslandEvent -= IslandOnCompleteIslandEvent;
        }

        private void Start()
        {
            Island.CompleteIslandEvent+= IslandOnCompleteIslandEvent;
        }

        private void IslandOnCompleteIslandEvent(Transform nextTarget)
        {
            _currentTarget = nextTarget;
            winPanel.ShowWinPanel(MoveToTarget);
        }

        private void MoveToTarget()
        {
            if(_currentTarget is null)
                return;
            cameraController.MoveToTarget(_currentTarget);
        }
    }
}
