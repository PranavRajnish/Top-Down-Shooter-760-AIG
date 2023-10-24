using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 10f;
        
        private PlayerInput _input;
        private Rigidbody2D _rb;

        private Vector2 _moveVector = Vector2.zero;
        private Vector3 _mousePosition = Vector3.zero;

        private Camera _levelCam;

        private void Awake()
        {
            _input = new PlayerInput();
            _rb = GetComponent<Rigidbody2D>();
            _levelCam = Camera.main;
            _mousePosition = _levelCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        }

        private void OnEnable()
        {
            _input.Enable();
            _input.Player.Movement.performed += OnMovementPerformed;
            _input.Player.Movement.canceled += OnMovementCancelled;

            _input.Player.Rotation.performed += OnMouseMoved;
        }


        private void OnDisable()
        {
            _input.Disable();
            _input.Player.Movement.performed -= OnMovementPerformed;
            _input.Player.Movement.canceled -= OnMovementCancelled;

            _input.Player.Rotation.performed -= OnMouseMoved;
        }

        private void OnMovementPerformed(InputAction.CallbackContext value)
        {
            _moveVector = value.ReadValue<Vector2>();
        }

        private void OnMovementCancelled(InputAction.CallbackContext value)
        {
            _moveVector = Vector2.zero;
        }

        private void FixedUpdate()
        {
            _rb.velocity = _moveVector * moveSpeed;
            
            var direction = _mousePosition - transform.position;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));
        }

        private void OnMouseMoved(InputAction.CallbackContext value)
        {
            _mousePosition = _levelCam.ScreenToWorldPoint(value.ReadValue<Vector2>());
        }
    }
}