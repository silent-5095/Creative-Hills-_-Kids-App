using System;
using Painting;
using UnityEngine;

namespace GameScene
{
    public class IslandCameraController : MonoBehaviour
    {
        private Vector3 _touchStart;
        [SerializeField] private new Camera camera;
        public float min, max;
        private bool _canTouch = true;
        private Vector3 _lastIslandCamLocation;

        private void OnDestroy()
        {
            QuestionPanel.QPanelRayCastEvent -= OnQPanelRayCastEvent;
            PlayerPrefs.SetFloat(nameof(_lastIslandCamLocation),camera.transform.localPosition.x);
        }

        private void Start()
        {
            QuestionPanel.QPanelRayCastEvent += OnQPanelRayCastEvent;
            _lastIslandCamLocation = camera.transform.localPosition;
            _lastIslandCamLocation.x = PlayerPrefs.HasKey(nameof(_lastIslandCamLocation))
                ? PlayerPrefs.GetFloat(nameof(_lastIslandCamLocation))
                : _lastIslandCamLocation.x;
            camera.transform.localPosition = _lastIslandCamLocation;
        }

        private void OnQPanelRayCastEvent(bool con)
        {
            _canTouch = !con;
        }

        private void Update()
        {
            if (!_canTouch)
                return;
            if (Input.GetMouseButtonDown(0))
            {
                _touchStart = camera.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.GetMouseButton(0))
            {
                var direction = _touchStart - camera.ScreenToWorldPoint(Input.mousePosition);
                var newPos = camera.transform.position;
                newPos.x += +direction.x;
                newPos.x = newPos.x > max ? max : newPos.x;
                newPos.x = newPos.x < min ? min : newPos.x;
                camera.transform.position = newPos;
            }
        }
    }
}