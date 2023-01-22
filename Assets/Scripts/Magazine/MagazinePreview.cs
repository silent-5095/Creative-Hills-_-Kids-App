using System;
using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Magazine
{
    public class MagazinePreview : MonoBehaviour
    {
        [SerializeField] private RTLTextMeshPro3D summary;
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private Button button;

        private void Start()
        {
            button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            
        }

        public void SetSummary(string value)
        {
            mainMenu.SetActive(true);
            summary.text = value;
        }
    }
}
