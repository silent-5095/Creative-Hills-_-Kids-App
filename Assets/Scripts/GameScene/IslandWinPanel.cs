using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene
{
    public class IslandWinPanel : WinPanel
    {
        [SerializeField] private Button[] submitButtons;
        private Action _currentAction;
        internal override void Start()
        {
            base.Start();
            foreach (var submitButton in submitButtons)
            {
                submitButton.onClick.AddListener(OnSubmitOnClickButton);
            }
        }

        public void ShowWinPanel(Action action)
        {
            _currentAction = action;
            mainPanel.SetActive(true);
            panels[CurrentIndex].SetActive(true);
            CurrentIndex = CurrentIndex < panels.Length - 1 ? CurrentIndex + 1 : 0;
            PlayerPrefs.SetInt(winPanel, CurrentIndex);
        }

        private void OnSubmitOnClickButton()
        {
            mainPanel.SetActive(false);
            _currentAction?.Invoke();
        }
    }
}