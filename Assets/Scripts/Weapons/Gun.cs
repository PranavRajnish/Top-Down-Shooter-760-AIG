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

        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform muzzle;
        [SerializeField] private float bulletDamage;

        private List<Bullet> _poolOfBullets;
        private Coroutine _shootingCoroutine;

        private const float NumberOfBullets = 20f;

        protected void Start()
        {
            _poolOfBullets = new List<Bullet>();
        }

        private void OnTriggerPulled()
        {
            if (_shootingCoroutine != null)
                StopCoroutine(_shootingCoroutine);

            _shootingCoroutine = StartCoroutine(Fire());
        }

        protected abstract IEnumerator Fire();

        protected void FireBullet()
        {
            if (_poolOfBullets.Count <= NumberOfBullets && _poolOfBullets.All(bullet => bullet.gameObject.activeSelf))
            {
                _poolOfBullets.Add(Instantiate(bulletPrefab, muzzle.position, quaternion.identity).Init(this));
            }

            foreach (var bullet in _poolOfBullets.Where(bullet => !bullet.gameObject.activeSelf))
            {
                bullet.OnBulletFired(muzzle.position, new Vector2(1, 0));
                break;
            }
        }

        private void OnTriggerReleased()
        {
            if (_shootingCoroutine != null)
                StopCoroutine(_shootingCoroutine);
        }
    }
}