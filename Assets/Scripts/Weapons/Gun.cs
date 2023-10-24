using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Player;
using Unity.Mathematics;
using UnityEngine;

namespace Weapons
{
    public abstract class Gun : MonoBehaviour
    {
        public PlayerWeaponControl WeaponWielder { get; private set; }
        public abstract GunType Type { get; }
        public abstract FireMode[] FireModes { get; }

        public float BulletDamage => bulletDamage;
        public Transform Muzzle => muzzle;

        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform muzzle;
        [SerializeField] private int magSize;
        [SerializeField] private float reloadTime = 10f;
        [SerializeField] private float bulletDamage;

        private int _bulletsRemaining;
        private List<Bullet> _poolOfBullets;
        private Coroutine _shootingCoroutine;
        private Coroutine _reloadCoroutine;

        protected void Start()
        {
            _poolOfBullets = new List<Bullet>();
            _bulletsRemaining = magSize;
            bulletPrefab.gameObject.SetActive(false);
        }

        public void OnTriggerPulled()
        {
            if (_shootingCoroutine != null)
                StopCoroutine(_shootingCoroutine);

            _shootingCoroutine = StartCoroutine(Fire());
        }

        protected abstract IEnumerator Fire();

        protected void FireBullet()
        {
            if (_bulletsRemaining <= 0)
                return;

            if (_poolOfBullets.Count <= magSize && _poolOfBullets.All(bullet => bullet.gameObject.activeSelf))
            {
                _poolOfBullets.Add(Instantiate(bulletPrefab, muzzle.position, quaternion.identity).Init(this));
            }

            foreach (var bullet in _poolOfBullets.Where(bullet => !bullet.gameObject.activeSelf))
            {
                _bulletsRemaining--;
                bullet.OnBulletFired();
                break;
            }
        }

        public void OnTriggerReleased()
        {
            if (_shootingCoroutine != null)
                StopCoroutine(_shootingCoroutine);
        }

        public void OnReloadPressed()
        {
            if (_reloadCoroutine == null && _bulletsRemaining < magSize)
                _reloadCoroutine = StartCoroutine(Reload());
        }

        private IEnumerator Reload()
        {
            yield return new WaitForSeconds(reloadTime);

            _bulletsRemaining = magSize;
            _reloadCoroutine = null;
        }
    }
}