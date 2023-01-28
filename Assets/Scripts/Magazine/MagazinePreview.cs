using System;
using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Magazine
{
    public class MagazinePreview : MonoBehaviour
    {
        public static MagazinePreview Instance;
        [SerializeField] private Image icon;
        [SerializeField] private RTLTextMeshPro3D summary;
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private Button button;

        private void Start()
        {
            Instance = this;
            button.onClick.AddListener(OnButtonClick);
        }

        
        private void LateUpdate()
        {
            if (Screen.orientation != ScreenOrientation.LandscapeLeft)
                ForDemo.Instance.ChangeOrientation(ScreenOrientation.LandscapeLeft);
        }

        private void OnButtonClick()
        {
            
        }

        public void SetSummary(MagazineData data)
        {
            mainMenu.SetActive(true);
            summary.text = data.Summary;
            icon.sprite = data.Icon;
        }
    }
}
