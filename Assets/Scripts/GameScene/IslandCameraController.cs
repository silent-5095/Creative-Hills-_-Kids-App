using System;
using Painting;
using UnityEngine;

namespace GameScene
{
    public class IslandCameraController : MonoBehaviour
    {
        private Vector3 _touchStart;
        [SerializeField] private new Camera camera;
        // public float zoomOutMin = 1;
        // public float zoomOutMax = 8;
        public float min,max;

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _touchStart = camera.ScreenToWorldPoint(Input.mousePosition);
            }

            // if (Input.touches.Length > 0)
            // {
            //     var touchZero = Input.GetTouch(0);
            //     var touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            //     if (Input.touchCount == 2)
            //     {
            //         var touchOne = Input.GetTouch(1);
            //
            //         var touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
            //
            //         var prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            //         var currentMagnitude = (touchZero.position - touchOne.position).magnitude;
            //
            //         var difference = currentMagnitude - prevMagnitude;
            //
            //         // Zoom(difference * 0.01f);
            //     }
            // }
            // else
                // Zoom(Input.GetAxis("Mouse ScrollWheel"));
            if (Input.GetMouseButton(0))
            {
                var direction = _touchStart - camera.ScreenToWorldPoint(Input.mousePosition);
                var newPos = camera.transform.position;
                newPos.x += +direction.x;
                newPos.x = newPos.x > max ? max : newPos.x;
                newPos.x = newPos.x < min ? min : newPos.x;
                // newPos.y = newPos.y > increment ? increment : newPos.y;
                // newPos.y = newPos.y < -increment ? -increment : newPos.y;
                camera.transform.position = newPos;
            }
        }

        // private void Zoom(float _increment)
        // {
        //     camera.orthographicSize = Mathf.Clamp(camera.orthographicSize - _increment, zoomOutMin, zoomOutMax);
        //     increment = zoomOutMax - camera.orthographicSize;
        // }
    }
}