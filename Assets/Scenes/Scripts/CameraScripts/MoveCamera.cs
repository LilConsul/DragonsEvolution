using UnityEngine;

namespace Scenes.Scripts.CameraScripts {
    public class MoveCamera : MonoBehaviour {
        private Vector3 _currentPosition;
        [SerializeField] private float cameraSpeed;
        [SerializeField] private float zoomSpeed;
        [SerializeField] private float minZoom;
        private Camera _camera;

        private void Start() {
            _camera = Camera.main;
        }

        private void Update() {
            _currentPosition = transform.position;

            var zoom = Input.GetAxis("Mouse ScrollWheel");
            _camera.orthographicSize = Mathf.Max(minZoom, _camera.orthographicSize - zoom * zoomSpeed);

            if (Input.GetKey(KeyCode.W))
                _currentPosition.y += cameraSpeed / 50;

            if (Input.GetKey(KeyCode.S))
                _currentPosition.y -= cameraSpeed / 50;

            if (Input.GetKey(KeyCode.A))
                _currentPosition.x -= cameraSpeed / 50;

            if (Input.GetKey(KeyCode.D))
                _currentPosition.x += cameraSpeed / 50;

            transform.position = _currentPosition;
        }
    }
}