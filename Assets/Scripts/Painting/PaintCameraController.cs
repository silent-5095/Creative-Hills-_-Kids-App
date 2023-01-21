using System;
using UnityEngine;

namespace Painting
{
    public class PaintCameraController : MonoBehaviour
    {
        public bool update;
        public new Camera camera;
        public RectTransform otherRect;
        private Vector2 _touch0, _touch1;

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

        private void Update()
        {
            Zoom();
        }

        private float _diff = 0, _tempDiff = 0, _touch0Def = 0, _touch1Def = 0;
        private bool _canZoom = false;

        private void Zoom()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _diff = Mathf.Abs(_touch0.magnitude - _touch1.magnitude) / 200;
            if (Input.GetMouseButtonDown(0))
            {
                _touch0 = Input.mousePosition;
                _touch0Def = _touch1Def = _touch0.magnitude;
                _diff = Mathf.Abs(_touch0.magnitude - _touch1.magnitude) / 200;
                _canZoom = true;
            }

            if (Input.GetMouseButton(0) && _canZoom)
            {
                _touch1 = Input.mousePosition;
                _tempDiff = Mathf.Abs(_touch0.magnitude - _touch1.magnitude) / 200;
                if (camera.orthographicSize < 10)
                {
                    Debug.Log($"touch0 magnitude ={_touch0.magnitude}  | touch1 magnitude = {_touch1.magnitude}");
                    if (_tempDiff < _diff)
                    {
                        _diff = _tempDiff;
                        // _diff = _diff / 100;
                        camera.orthographicSize = camera.orthographicSize + _diff;
                    }
                }

                if (camera.orthographicSize > 4)
                {
                    if (_tempDiff > _diff)
                    {
                        _diff = _tempDiff;
                        // _diff = _diff / 100;
                        camera.orthographicSize = camera.orthographicSize - _diff;
                    }
                }


                // _tempDiff = Mathf.Abs(_touch0.magnitude - _touch1.magnitude) / 200;
                // if (_tempDiff > _diff)
                // {
                //     _diff = _tempDiff;
                //     // _diff = _diff / 100;
                //     camera.orthographicSize = camera.orthographicSize - _diff;
                // }
                // else if (_tempDiff < _diff)
                // {
                //     _diff = _tempDiff;
                //     // _diff = _diff / 100;
                //     camera.orthographicSize = camera.orthographicSize + _diff;
                // }
            }

            // if (Input.GetMouseButtonUp(0))
            // {
            //     _canZoom = false;
            //     var diff = Mathf.Abs(_touch0.magnitude - _touch1.magnitude);
            //     Debug.Log($"diff ={diff}");
            // }

            // if (Input.touchCount > 2)
            // {
            //     if (Input.touches[0].phase == TouchPhase.Moved)
            //     {
            //         _touch0 = Input.touches[0].position;
            //     }
            //
            //     if (Input.touches[1].phase == TouchPhase.Moved)
            //     {
            //         _touch1 = Input.touches[1].position;
            //     }
            //
            //     if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[1].phase == TouchPhase.Ended)
            //     {
            //         var diff = Mathf.Abs(_touch0.magnitude - _touch1.magnitude);
            //         Debug.Log($"diff ={diff}");
            //     }
            // }
        }
    }
}