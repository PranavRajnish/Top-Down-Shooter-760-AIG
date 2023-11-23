using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapons;

namespace Player
{
    public class PlayerWeaponControl : PlayerComponent
    {
        [SerializeField] private Gun[] gunOptions;
        [SerializeField] TextMeshProUGUI reloadText;

        public Gun CurrentGun
        {
            get => _currentGun;
            private set
            {
                if (!value) return;

                if (_currentGun)
                    _currentGun.gameObject.SetActive(false);
                _currentGun = value;
                _currentGun.gameObject.SetActive(true);
            }
        }

        private Gun _currentGun;
        private PlayerInput _input;

        private void Awake()
        {
            _input = new PlayerInput();
            foreach (var gun in gunOptions)
                gun.gameObject.SetActive(false);

            CurrentGun = gunOptions[0];
        }

        private void OnEnable()
        {
            _input.Enable();
            CurrentGun.gameObject.SetActive(true);

            _input.Player.Attack.performed += OnPlayerAttack;
            _input.Player.Attack.canceled += OnPlayerAttackEnd;

            _input.Player.Reload.performed += OnPlayerReload;

            _input.Player.ADS.performed += OnPlayerADS;
            _input.Player.ADS.canceled += OnPlayerADSEnd;

            _input.Player.GunChange.performed += OnPlayerGunChange;
        }


        private void OnDisable()
        {
            _input.Disable();
            CurrentGun.gameObject.SetActive(false);

            _input.Player.Attack.performed -= OnPlayerAttack;
            _input.Player.Attack.canceled -= OnPlayerAttackEnd;

            _input.Player.Reload.performed += OnPlayerReload;

            _input.Player.ADS.performed -= OnPlayerADS;
            _input.Player.ADS.canceled -= OnPlayerADSEnd;

            _input.Player.GunChange.performed += OnPlayerGunChange;
        }

        private void OnPlayerAttack(InputAction.CallbackContext value)
        {
            Debug.Log("Shoot Start");
            CurrentGun.OnTriggerPulled();
        }

        private void OnPlayerAttackEnd(InputAction.CallbackContext value)
        {
            Debug.Log("Shoot End");
            CurrentGun.OnTriggerReleased();
        }

        private void OnPlayerReload(InputAction.CallbackContext value)
        {
            Debug.Log("Reload Start");
            CurrentGun.OnReloadPressed();
        }

        private void OnPlayerADS(InputAction.CallbackContext obj)
        {
            Debug.Log("ADS End");
        }

        private void OnPlayerADSEnd(InputAction.CallbackContext obj)
        {
            Debug.Log("ADS Start");
        }

        private void OnPlayerGunChange(InputAction.CallbackContext value)
        {
            var readValue = (int)value.ReadValue<float>() / 120;
            Debug.Log($"Start : {readValue}");

            ChangeGun(readValue % gunOptions.Length);
        }

        private void ChangeGun(int increment)
        {
            var currentGunIndex = Array.IndexOf(gunOptions, CurrentGun);
            currentGunIndex += increment;
            currentGunIndex %= gunOptions.Length;

            CurrentGun = gunOptions[currentGunIndex];
        }

        private void Update()
        {
            if (CurrentGun && reloadText)
                reloadText.SetText(CurrentGun.BulletsRemaining == 0 ? "Press 'R' To Reload" : "");
        }
    }
}