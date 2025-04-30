using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private Transform cameraTarget;
        [SerializeField] private float keyboardPanSpeed = 5;
        [SerializeField] private CinemachineCamera cinemachineCamera;
        [SerializeField] private float zoomSpeed = 1;
        [SerializeField] private float rotationSpeed = 1;
        [SerializeField] private float minZoomDistance = 7.5f;

        private CinemachineFollow _cinemachineFollow;
        private float _maxRotationAmount;
        private float _rotationStartTime;
        private Vector3 _startingFollowOffset;
        private float _zoomStartTime;

        private static bool ShouldSetZoomStartTime => Keyboard.current.endKey.wasPressedThisFrame ||
                                                      Keyboard.current.endKey.wasReleasedThisFrame;

        private static bool ShouldSetRotationStartTime => Keyboard.current.pageUpKey.wasPressedThisFrame ||
                                                          Keyboard.current.pageDownKey.wasPressedThisFrame ||
                                                          Keyboard.current.pageUpKey.wasReleasedThisFrame ||
                                                          Keyboard.current.pageDownKey.wasReleasedThisFrame;

        private void Awake()
        {
            if (!cinemachineCamera.TryGetComponent(out _cinemachineFollow))
                Debug.LogError("Cinemachine Camera did not have CinemachineFollow. Zoom functionality will not work!");

            _startingFollowOffset = _cinemachineFollow.FollowOffset;
            _maxRotationAmount = Mathf.Abs(_cinemachineFollow.FollowOffset.z);
        }

        private void Update()
        {
            HandlePanning();
            HandleZooming();
            HandleRotation();
        }

        private void HandleRotation()
        {
            if (ShouldSetRotationStartTime) _rotationStartTime = Time.time;

            var rotationTime = Mathf.Clamp01((Time.time - _rotationStartTime) * rotationSpeed);

            Vector3 targetFollowOffset;
            if (Keyboard.current.pageDownKey.isPressed)
                targetFollowOffset = new Vector3(_maxRotationAmount, _cinemachineFollow.FollowOffset.y, 0);
            else if (Keyboard.current.pageUpKey.isPressed)
                targetFollowOffset = new Vector3(-_maxRotationAmount, _cinemachineFollow.FollowOffset.y, 0);
            else
                targetFollowOffset = new Vector3(_startingFollowOffset.x, _cinemachineFollow.FollowOffset.y,
                    _startingFollowOffset.z);

            _cinemachineFollow.FollowOffset =
                Vector3.Slerp(_cinemachineFollow.FollowOffset, targetFollowOffset, rotationTime);
        }

        private void HandleZooming()
        {
            if (ShouldSetZoomStartTime) _zoomStartTime = Time.time;

            Vector3 targetFollowOffset;

            var zoomTime = Mathf.Clamp01(zoomSpeed * (Time.time - _zoomStartTime));
            if (Keyboard.current.endKey.isPressed)
                targetFollowOffset = new Vector3(_cinemachineFollow.FollowOffset.x, minZoomDistance,
                    _cinemachineFollow.FollowOffset.z);
            else
                targetFollowOffset = new Vector3(_cinemachineFollow.FollowOffset.x, _startingFollowOffset.y,
                    _cinemachineFollow.FollowOffset.z);

            _cinemachineFollow.FollowOffset = Vector3.Slerp(
                _cinemachineFollow.FollowOffset, targetFollowOffset, zoomTime);
        }

        private void HandlePanning()
        {
            var moveAmount = Vector2.zero;

            if (Keyboard.current.upArrowKey.isPressed) moveAmount.y += keyboardPanSpeed;
            if (Keyboard.current.leftArrowKey.isPressed) moveAmount.x -= keyboardPanSpeed;
            if (Keyboard.current.downArrowKey.isPressed) moveAmount.y -= keyboardPanSpeed;
            if (Keyboard.current.rightArrowKey.isPressed) moveAmount.x += keyboardPanSpeed;

            moveAmount *= Time.deltaTime;
            cameraTarget.position += new Vector3(moveAmount.x, 0, moveAmount.y);
        }
    }
}