using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Painting
{
    public class PaintCameraController : MonoBehaviour
    {
        private Vector3 _touchStart;
        public static PaintCameraController Instance;
        [SerializeField] private new Camera camera;
        public float CameraSize => camera.orthographicSize;
        public float zoomOutMin = 1;
        public float zoomOutMax = 8;
        public float increment;

        // Update is called once per frame
        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _touchStart = camera.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.touches.Length > 0)
            {
                var touchZero = Input.GetTouch(0);
                var touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                if (Input.touchCount == 2)
                {
                    var touchOne = Input.GetTouch(1);

                    var touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                    var prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                    var currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                    var difference = currentMagnitude - prevMagnitude;

                    Zoom(difference * 0.01f);
                }
            }
            else
                Zoom(Input.GetAxis("Mouse ScrollWheel"));
            if (Input.GetMouseButton(0))
            {
                var direction = _touchStart - camera.ScreenToWorldPoint(Input.mousePosition);
                var newPos = camera.transform.position + direction;
                newPos.x = newPos.x > increment ? increment : newPos.x;
                newPos.x = newPos.x < -increment ? -increment : newPos.x;
                newPos.y = newPos.y > increment ? increment : newPos.y;
                newPos.y = newPos.y < -increment ? -increment : newPos.y;
                camera.transform.position = newPos;
            }
        }

        public void Zoom(float currentIncrement)
        {
            Debug.Log($"Zoom {currentIncrement}");
            camera.orthographicSize = Mathf.Clamp(camera.orthographicSize - currentIncrement, zoomOutMin, zoomOutMax);
            increment = zoomOutMax - camera.orthographicSize;
        }
    }
}