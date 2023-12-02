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

        private Camera _levelCam;

        private void Awake()
        {
            _input = new PlayerInput();
            _rb = GetComponent<Rigidbody2D>();
            _levelCam = Camera.main;
            _levelCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        }

        private void OnEnable()
        {
            _input.Enable();
            _input.Player.Movement.performed += OnMovementPerformed;
            _input.Player.Movement.canceled += OnMovementCancelled;
        }


        private void OnDisable()
        {
            _input.Disable();
            _input.Player.Movement.performed -= OnMovementPerformed;
            _input.Player.Movement.canceled -= OnMovementCancelled;
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

            var mousePosition = _levelCam.ScreenToWorldPoint(Mouse.current.position.value);
            var direction = mousePosition - transform.position;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));
        }
    }
}