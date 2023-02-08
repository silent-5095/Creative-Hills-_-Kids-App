using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameScene
{
    public class IslandManager : MonoBehaviour
    {
        [SerializeField] private List<Island> islands;
        [SerializeField] private IslandCameraController cameraController;
        [FormerlySerializedAs("winPanel")] [SerializeField] private IslandWinPanel gameWinPanel;
        [SerializeField] private IslandWinPanel mattrahWinPanel,soharWinPanel,nizwaWinPanel;
        private Transform _currentTarget;

        private void OnDestroy()
        {
            Island.CompleteIslandEvent -= IslandOnCompleteIslandEvent;
        }

        private void Start()
        {
            Island.CompleteIslandEvent+= IslandOnCompleteIslandEvent;
        }
        private void IslandOnCompleteIslandEvent(Transform nextTarget,IslandType islandType)
        {
            _currentTarget = nextTarget;
            switch (islandType)
            {
                case IslandType.Mattrah:
                    mattrahWinPanel.ShowWinPanel(MoveToTarget);
                    break;
                case IslandType.Sohar:
                    soharWinPanel.ShowWinPanel(MoveToTarget);
                    break;
                case IslandType.Nizwa:
                    nizwaWinPanel.ShowWinPanel(MoveToTarget);
                    break;
                case IslandType.Game:
                    gameWinPanel.ShowWinPanel(MoveToTarget);
                    break;
                default:
                    gameWinPanel.ShowWinPanel(MoveToTarget);
                    break;
            }
        }

        private void MoveToTarget()
        {
            if(_currentTarget is null)
                return;
            cameraController.MoveToTarget(_currentTarget);
        }
    }
}
