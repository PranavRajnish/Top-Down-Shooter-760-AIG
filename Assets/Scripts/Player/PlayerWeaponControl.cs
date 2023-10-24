using UnityEngine;
using UnityEngine.InputSystem;
using Weapons;

namespace Player
{
    public class PlayerWeaponControl : PlayerComponent
    {
        private PlayerInput _input;
        [SerializeField] private Gun _currentGun;

        private void Awake()
        {
            _input = new PlayerInput();
        }

        private void OnEnable()
        {
            _input.Enable();
            _input.Player.Attack.performed += OnPlayerAttack;
            _input.Player.Attack.canceled += OnPlayerAttackEnd;

            _input.Player.Reload.performed += OnPlayerReload;
            
            _input.Player.ADS.performed += OnPlayerADS;
            _input.Player.ADS.canceled += OnPlayerADSEnd;
        }


        private void OnDisable()
        {
            _input.Disable();
            _input.Player.Attack.performed -= OnPlayerAttack;
            _input.Player.Attack.canceled -= OnPlayerAttackEnd;

            _input.Player.Reload.performed += OnPlayerReload;
            
            _input.Player.ADS.performed -= OnPlayerADS;
            _input.Player.ADS.canceled -= OnPlayerADSEnd;
        }

        private void OnPlayerAttack(InputAction.CallbackContext value)
        {
            Debug.Log("Shoot Start");
            _currentGun.OnTriggerPulled();
        }

        private void OnPlayerAttackEnd(InputAction.CallbackContext value)
        {
            Debug.Log("Shoot End");
            _currentGun.OnTriggerReleased();
        }
        private void OnPlayerReload(InputAction.CallbackContext value)
        {
            Debug.Log("Reload Start");
            _currentGun.OnReloadPressed();
        }

        private void OnPlayerADSEnd(InputAction.CallbackContext obj)
        {
            Debug.Log("ADS Start");
        }

        private void OnPlayerADS(InputAction.CallbackContext obj)
        {
            Debug.Log("ADS End");
        }
    }
}