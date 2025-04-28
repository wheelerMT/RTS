using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private Transform cameraTarget;
        [SerializeField] private float keyboardPanSpeed = 5;

        // Update is called once per frame
        private void Update()
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