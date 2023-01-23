using Painting;
using UnityEngine;
using UnityEngine.UI;

namespace Magazine
{
    // [RequireComponent(typeof(Scrollbar))]
    public class MagazineScrollBar : MonoBehaviour
    {
        [SerializeField] private float zoomStep,buttonZoomSpeed;
        [SerializeField] private Button scrollbarActivator,zoomIn,zoomOut;
        [SerializeField] private Scrollbar scrollbar;

        private void Start()
        {
            scrollbarActivator.onClick.AddListener(() => scrollbar.gameObject.SetActive(!scrollbar.gameObject.activeSelf));
            zoomIn.onClick.AddListener(ZoomIn);
            zoomOut.onClick.AddListener(ZoomOut);
            zoomStep = (PaintCameraController.Instance.zoomOutMax - PaintCameraController.Instance.zoomOutMin) / 100;
        }
        

        private float _scrollBarPreValue = 0;
        public void OnScrollBar()
        {
            var value = scrollbar.value;
            Debug.Log(value);
            // value *= _scrollBarPreValue < scrollbar.value ? 1 : -1;
            value -=_scrollBarPreValue;
            _scrollBarPreValue = scrollbar.value;
            PaintCameraController.Instance.Zoom(value*zoomStep*100);
        }

        private void ZoomIn()
        {
            PaintCameraController.Instance.Zoom(buttonZoomSpeed);
        }

        private void ZoomOut()
        {
            
            PaintCameraController.Instance.Zoom(-buttonZoomSpeed);
        }
    }
}