using System;
using UnityEngine;

namespace Painting
{
    public class PaintCameraController : MonoBehaviour
    {
        public bool update;
        public new Camera camera;
        public RectTransform otherRect;

        private void OnValidate()
        {
            if (update)
            {
                update = false;
                Debug.Log(camera.rect.size);
                Debug.Log(camera.rect.position);
                Debug.Log(camera.rect.Overlaps(otherRect.rect));
                Debug.Log(Screen.width);
                Debug.Log(Screen.height);
            }
        }
    }
}
