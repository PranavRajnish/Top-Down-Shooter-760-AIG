using UnityEngine;
using UnityEngine.InputSystem;
using Weapons;

namespace Player
{
    public class PlayerWeaponControl : PlayerComponent
    {
        private PlayerInput _input;
        private Gun _currentGun;
    
        private void Awake()
        {
            _input = new PlayerInput();
        }

        private void OnEnable()
        {
            _input.Enable();
            _input.Player.Attack.performed += OnPlayerAttack;
            _input.Player.Attack.canceled += OnPlayerAttackEnd;

            _input.Player.ADS.performed += OnPlayerADS;
            _input.Player.ADS.canceled += OnPlayerADSEnd;
        }


        private void OnDisable()
        {
            _input.Disable();
            _input.Player.Attack.performed -= OnPlayerAttack;
            _input.Player.Attack.canceled -= OnPlayerAttackEnd;

            _input.Player.ADS.performed -= OnPlayerADS;
            _input.Player.ADS.canceled -= OnPlayerADSEnd;
        }

        private void OnPlayerAttack(InputAction.CallbackContext value)
        {
            Debug.Log("Shoot Start");
        }

        private void OnPlayerAttackEnd(InputAction.CallbackContext value)
        {
            Debug.Log("Shoot End");
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