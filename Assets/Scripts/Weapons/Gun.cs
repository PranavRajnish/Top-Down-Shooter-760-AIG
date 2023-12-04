using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Weapons
{
    public abstract class Gun : MonoBehaviour
    {
        public abstract GunType Type { get; }
        public abstract FireMode[] FireModes { get; }
        public FireMode CurrentFireMode { get; protected set; }
        public float BulletDamage => bulletDamage;
        public Transform Muzzle => muzzle;
        public bool IsShooting => _shootingCoroutine != null;
        public bool IsReloading => _reloadCoroutine != null;
        public int BulletsRemaining { get; private set; }
        public float NormalizedBulletsRemaining => (float)BulletsRemaining / magSize;

        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform muzzle;
        [SerializeField] private int magSize;
        [SerializeField] private float reloadTime = 10f;
        [SerializeField] private float bulletDamage;

        private List<Bullet> _poolOfBullets;
        private Coroutine _shootingCoroutine;
        private Coroutine _reloadCoroutine;
        private bool _triggerPulled;

        protected virtual void Start()
        {
            _poolOfBullets = new List<Bullet>();
            BulletsRemaining = magSize;
            bulletPrefab.gameObject.SetActive(false);
        }

        public void OnTriggerPulled()
        {
            if (_triggerPulled || IsReloading)
                return;

            if (IsShooting)
                StopCoroutine(_shootingCoroutine);

            _triggerPulled = true;
            _shootingCoroutine = StartCoroutine(Fire());
        }

        protected abstract IEnumerator Fire();

        protected void FireBullet()
        {
            if (BulletsRemaining <= 0)
                return;

            if (_poolOfBullets.Count <= magSize && _poolOfBullets.All(bullet => bullet.gameObject.activeSelf))
                _poolOfBullets.Add(Instantiate(bulletPrefab, muzzle.position, quaternion.identity).Init(this));

            foreach (var bullet in _poolOfBullets.Where(bullet => !bullet.gameObject.activeSelf))
            {
                BulletsRemaining--;
                bullet.OnBulletFired();
                break;
            }
        }

        public void OnTriggerReleased()
        {
            if (IsShooting)
                StopCoroutine(_shootingCoroutine);

            _triggerPulled = false;
        }

        public void OnReloadPressed()
        {
            if (IsShooting)
                OnTriggerReleased();

            if (!IsReloading && BulletsRemaining < magSize)
                _reloadCoroutine = StartCoroutine(Reload());
        }

        private void OnDestroy()
        {
            foreach (var bullet in _poolOfBullets)
                DestroyImmediate(bullet);
        }

        private IEnumerator Reload()
        {
            yield return new WaitForSeconds(reloadTime);

            BulletsRemaining = magSize;
            _reloadCoroutine = null;
        }
    }
}